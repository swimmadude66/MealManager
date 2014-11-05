#!/usr/bin/perl
# sendDataToDB.pl
use warnings;
use utf8;
binmode( STDOUT, ': utf8' );
require '../../SiteScrapers/common.pl';

my $stamp = strftime( "%Y%m%d", localtime );
my $i;

my $database = 'adamyost_Inventory';			#'mealmanager';
my $host = 'adamYost.writhem.com'; 				#'meal-manager-cs-4911.chplv1mpw4j4.us-west-2.rds.amazonaws.com';
my $port = '3306';
my $user = 'adamyost_asalt'; 					#'recipesrc';
my $pass = 'CS4911B'; 							#'giveMeMeals2014';
my $db_handle = db_connect( $database, $host, $port, $user, $pass ); 

# my $curr_part = 1;
# my $part_count = 6;

my %recipe = (
	'Name' 		    , '',
	'Description'   , '',
	'Yield'	        , '',
	'Calories'	    , 'NULL',
	'Carbohydrates' , 'NULL',
	'Cholesterol'   , 'NULL',
	'Fat'		    , 'NULL',
	'Fiber' 	    , 'NULL',
	'Protein'       , 'NULL',
	'Sodium'        , 'NULL',
	'PrepTime'      , '',
	'CookTime'      , '',
	'Directions'    , '',
	'URL'           , ''
);

my $recipes_inserted = 0;
my @recipe_items = ();
my $query = '';
my $query_handle;
my $x = 0;

for( $x = 1; $x <= 6; $x++ )
{
	open RECIPES, 'RecipeData' . $x . 'of6.csv';

	print "------------------------------------\n";
	print "Starting Part $x of 6\n";
	print "------------------------------------\n";

	while( my $line = <RECIPES> )
	{
		my @parts = split( /\|/, $line );

		if( is_missing_or_empty( $recipe{ 'Name' } ) )
		{
			$recipe{ 'Name' } = escape( $parts[0] );
		}
		else 
		{
			if( $recipe{ 'Name' } ne escape( $parts[0] ) ){
				#print Dumper( %recipe );
				
				# foreach my $item ( @recipe_items )
				# {
				# 	print Dumper( $item );
				# 	print "-------------------------------\n";
				# }

				# PREPARE THE QUERY
				$query = 'INSERT INTO `adamyost_Inventory`.`Recipe`
					(`ID`, `Name`, `Description`, `Yield`, 
					`Calories`, `Carbohydrates`, `Cholesterol`, `Fat`, `Fiber`, `Protein`, `Sodium`,
					`PrepTime`, `CookTime`, `Directions`, `URL`)
					VALUES
					(NULL, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? )';
				$query_handle = $db_handle->prepare($query);

				# EXECUTE THE QUERY
				$query_handle->execute($recipe{'Name'}, 
					$recipe{'Description'}, 
					$recipe{'Yield'}, 
					$recipe{'Calories'}, 
					$recipe{'Carbohydrates'}, 
					$recipe{'Cholesterol'}, 
					$recipe{'Fat'}, 
					$recipe{'Fiber'}, 
					$recipe{'Protein'}, 
					$recipe{'Sodium'}, 
					$recipe{'PrepTime'}, 
					$recipe{'CookTime'}, 
					$recipe{'Directions'},  
					$recipe{'URL'}
				);

				my $recipe_id = $query_handle->{mysql_insertid};

				foreach my $curr_item ( @recipe_items )
				{
					my %item = %{ $curr_item };
					$query = 'INSERT INTO `adamyost_Inventory`.`RecipeItem`
						(`ID`, `Quantity`, `MeasureID`,	`MeasureDescription`, `IngredientID`, `Description`, `RecipeID`)
						VALUES
						(NULL, ?, ?, ?, ?, ?, ?)';
					$query_handle = $db_handle->prepare($query);

					$query_handle->execute($item{'Quantity'}, 
						$item{'MeasureID'},
						$item{'MeasureDescription'},
						$item{'IngredientID'},
						$item{'Description'},
						$recipe_id
					);
				}

				$recipes_inserted++;

				print "$recipes_inserted Recipes Inserted Into DB\n";

				$recipe_id = -1;

				%recipe = (
					'Name' 		    , escape( $parts[0] ),
					'Description'   , '',
					'Yield'	        , '',
					'Calories'	    , 'NULL',
					'Carbohydrates' , 'NULL',
					'Cholesterol'   , 'NULL',
					'Fat'		    , 'NULL',
					'Fiber' 	    , 'NULL',
					'Protein'       , 'NULL',
					'Sodium'        , 'NULL',
					'PrepTime'      , '',
					'CookTime'      , '',
					'Directions'    , '',
					'URL'           , ''
				);

				@recipe_items = (); 
			}
		}

		if( $parts[1] eq "Cook Time" )
		{
			if( is_missing_or_empty( $parts[2] ) )
			{
				$recipe{ 'CookTime' } = "NULL";
			}
			$recipe{ 'CookTime' } = escape( $parts[2] );
		}
		elsif( $parts[1] eq "Description" )
		{
			if( is_missing_or_empty( $parts[2] ) )
			{
				$recipe{ 'Description' } = "NULL";
			}
			$recipe{ 'Description' } = escape( $parts[2] );
		}
		elsif( $parts[1] eq "Prep Time" )
		{
			if( is_missing_or_empty( $parts[2] ) )
			{
				$recipe{ 'PrepTime' } = "NULL";
			}
			$recipe{ 'PrepTime' } = escape( $parts[2] );
		}
		elsif( $parts[1] eq "URL" )	     
		{
			if( is_missing_or_empty( $parts[2] ) )
			{
				$recipe{ 'URL' } = "NULL";
			}
			$recipe{ 'URL' } = escape( $parts[2] );
		}
		elsif( $parts[1] eq "Yield" )
		{
			if( is_missing_or_empty( $parts[2] ) )
			{
				$recipe{ 'Yield' } = "NULL";
			}
			$recipe{ 'Yield' } = escape( $parts[2] );
		}
		elsif( $parts[1] eq "Direction" )
		{
			my $step 		      = $parts[2] + 1;
			my $cleaned_direction = $step . ") " . escape( $parts[3] ) . "\n";

			$recipe{ 'Directions' } .= $cleaned_direction ;
		}
		elsif( $parts[1] eq "Nutrition" )
		{
			my $type   = $parts[2];
			if( $parts[3] =~ /^(\d+\.?\d*)\s/ )
			{
				$recipe{ $type } = $1;
			}
		}
		elsif( $parts[1] eq "Ingredient" )
		{
			my %curr_ingredient = ();

			if( is_missing_or_empty( $parts[3] ) )
			{
				# 
				$curr_ingredient{ 'IngredientID' } = get_ingredient_id( $parts[2] );
				$curr_ingredient{ 'Description' }  = escape( $parts[2] );
				$curr_ingredient{ 'MeasureID' } = 83;
				$curr_ingredient{ 'MeasureDescription' } = '';
				$curr_ingredient{ 'Quantity' } = 1;
			} 
			else
			{
				# 3 --> Q + M
				my %amount = split_amount( escape( $parts[2] ) );
				$curr_ingredient{ 'MeasureID' } = get_measure_id( $amount{'measure'} );
				$curr_ingredient{ 'MeasureDescription' } = $amount{'measure'};
				$curr_ingredient{ 'Quantity' } = $amount{'quantity'};

				# 4 --> Ingredient
				$curr_ingredient{ 'IngredientID' } = get_ingredient_id( $parts[3] );
				$curr_ingredient{ 'Description' }  = escape( $parts[3] );
			}

			push( @recipe_items, \%curr_ingredient );
		}
	}

	close RECIPES;

	# PREPARE THE QUERY
	$query = 'INSERT INTO `adamyost_Inventory`.`Recipe`
		(`ID`, `Name`, `Description`, `Yield`, 
		`Calories`, `Carbohydrates`, `Cholesterol`, `Fat`, `Fiber`, `Protein`, `Sodium`,
		`PrepTime`, `CookTime`, `Directions`, `URL`)
		VALUES
		(NULL, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? )';
	$query_handle = $db_handle->prepare($query);

	# EXECUTE THE QUERY
	$query_handle->execute($recipe{'Name'}, 
		$recipe{'Description'}, 
		$recipe{'Yield'}, 
		$recipe{'Calories'}, 
		$recipe{'Carbohydrates'}, 
		$recipe{'Cholesterol'}, 
		$recipe{'Fat'}, 
		$recipe{'Fiber'}, 
		$recipe{'Protein'}, 
		$recipe{'Sodium'}, 
		$recipe{'PrepTime'}, 
		$recipe{'CookTime'}, 
		$recipe{'Directions'},  
		$recipe{'URL'}
	);

	my $recipe_id = $query_handle->{mysql_insertid};

	foreach my $curr_item ( @recipe_items )
	{
		my %item = %{ $curr_item };
		$query = 'INSERT INTO `adamyost_Inventory`.`RecipeItem`
			(`ID`, `Quantity`, `MeasureID`,	`MeasureDescription`, `IngredientID`, `Description`, `RecipeID`)
			VALUES
			(NULL, ?, ?, ?, ?, ?, ?)';
		$query_handle = $db_handle->prepare($query);

		$query_handle->execute($item{'Quantity'}, 
			$item{'MeasureID'},
			$item{'MeasureDescription'},
			$item{'IngredientID'},
			$item{'Description'},
			$recipe_id
		);
	}

	$recipes_inserted++;

	print "$recipes_inserted Recipes Inserted Into DB\n";

	$recipe_id = -1;

	%recipe = (
		'Name' 		    , '',
		'Description'   , '',
		'Yield'	        , '',
		'Calories'	    , 'NULL',
		'Carbohydrates' , 'NULL',
		'Cholesterol'   , 'NULL',
		'Fat'		    , 'NULL',
		'Fiber' 	    , 'NULL',
		'Protein'       , 'NULL',
		'Sodium'        , 'NULL',
		'PrepTime'      , '',
		'CookTime'      , '',
		'Directions'    , '',
		'URL'           , ''
	);

	@recipe_items = (); 
}

db_disconnect();
