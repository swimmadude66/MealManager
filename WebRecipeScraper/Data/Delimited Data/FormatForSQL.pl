#!/usr/bin/perl
# sendDataToDB.pl
use warnings;
use utf8;
binmode( STDOUT, ': utf8' );
require '../../SiteScrapers/common.pl';

my $stamp = strftime( "%Y%m%d", localtime );
my $i;

my $database = 'mealmanager';
my $host = 'meal-manager-cs-4911.chplv1mpw4j4.us-west-2.rds.amazonaws.com';
my $port = '3306';
my $user = 'recipesrc';
my $pass = 'giveMeMeals2014';
my $database_handle = db_connect( $database, $host, $port, $user, $pass ); 

# my $curr_part = 1;
# my $part_count = 6;

my %recipe = (
	'Name' 		    => '',
	'Description'   => '',
	'Yield'	        => '',
	'Calories'	    => '',
	'Carbohydrates' => '',
	'Cholesterol'   => '',
	'Fat'		    => '',
	'Fiber' 	    => '',
	'Protein'       => '',
	'Sodium'        => '',
	'PrepTime'      => '',
	'CookTime'      => '',
	'Directions'    => (),
	'URL'           => ''
);

my @ingredients = ();

# for( $curr_part = 0; $curr_part < $part_count; $curr_part++ )
# {
	# my @ingredients = ( Quantity, MeasureStr, IngredientStr );

	#open RECIPES, 'RecipeData' . $curr_part . 'of' . $part_count . '.csv';
	open RECIPES, 'test';

	while( my $line = <RECIPES> )
	{
		my @parts = split( /|/, $line );

		if( !defined( $recipe{ 'Name' } ) )
		{
			$recipe{ 'Name' } = escape( $parts[0] );
		}
		else 
		{
			if( $recipe{ 'Name' } ne escape( $parts[0] ) ){
				print Dumper( %recipe );
				print Dumper( @ingredients );

				%recipe = (
					'Name' 		    => escape( $parts[0] ),
					'Description'   => '',
					'Yield'	        => '',
					'Calories'	    => '',
					'Carbohydrates' => '',
					'Cholesterol'   => '',
					'Fat'		    => '',
					'Fiber' 	    => '',
					'Protein'       => '',
					'Sodium'        => '',
					'PrepTime'      => '',
					'CookTime'      => '',
					'Directions'    => (),
					'URL'           => ''
				);

				@ingredients = (); 
			}
		}

		switch( $parts[1] ){
			case "Cook Time"     
			{
				if( is_missing_or_empty( $parts[2] ) )
				{
					undef $recipe{ 'CookTime' };
				}
				$recipe{ 'CookTime' } = escape( $parts[2] );
			}
			case "Description"   
			{
				if( is_missing_or_empty( $parts[2] ) )
				{
					undef $recipe{ 'Description' };
				}
				$recipe{ 'Description' } = escape( $parts[2] );
			}
			case "Prep Time"     {
				if( is_missing_or_empty( $parts[2] ) )
				{
					undef $recipe{ 'PrepTime' };
				}
				$recipe{ 'PrepTime' } = escape( $parts[2] );
			}
			case "URL"		     
			{
				if( is_missing_or_empty( $parts[2] ) )
				{
					undef $recipe{ 'URL' };
				}
				$recipe{ 'URL' } = escape( $parts[2] );
			}
			case "Yield"	     
			{
				if( is_missing_or_empty( $parts[2] ) )
				{
					undef $recipe{ 'Yield' };
				}
				$recipe{ 'Yield' } = escape( $parts[2] );
			}
			case "Direction"     
			{
				my $step 		      = $parts[2] + 1;
				my $cleaned_direction = $step . ") " . escape( $parts[3] );

				push( @{ $recipe{ 'Directions' } }, $cleaned_direction );
			}
			case "Nutrition"     
			{
				my $type   = $parts[2];
				if( $parts[3] =~ /^(\d+\.?\d*)\s/ )
				{
					$recipe{ $type } = $1;
				}
			}
			case "Ingredient"
			{
				if( is_missing_or_empty( $part[3] ) )
				{
					# no Q + M
				}
			}
		}
	}

	close RECIPES;
# }