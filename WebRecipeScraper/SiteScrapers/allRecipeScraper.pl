#!/usr/bin/perl
# allRecipeScraper.pl
use warnings;
use utf8;
binmode( STDOUT, ': utf8' );

use Web::Scraper::LibXML;
require 'SiteScrapers/common.pl';

my $i;
my $logger = get_logger();
#our $recipe_ptr;

# Parts we have to retrieve and clean up
#	Recipe Name
#	Recipe Description
#	Recipe Yield
#	Ingredient List
#		- Name
#		- Description
#		- Quantity
#		- Measure
# 	Directions
#	Prep Time ( Minutes and Hours )
#	Cook Time ( Minutes and Hours )
my $scraper_parts = scraper {
	process "h1#itemTitle", 'recipe_name' => 'TEXT';
	process "span#lblDescription", 'recipe_description' => 'TEXT';
	process "span#lblYield", 'recipe_yield' => 'TEXT';
	process "ul.ingredient-wrap > li > label > p.fl-ing", "ingredients[]" => scraper( sub {
		process "span.ingredient-amount", 'amount' => 'TEXT';
		process "span.ingredient-name", 'name_desc' => 'TEXT';
	} ); 
	process "div.directions > div.directLeft > ol > li", 'recipe_directions[]' => 'TEXT';
	process "li#liPrep > span#prepMinsSpan > em", "prep_time_min" => 'TEXT';
	process "li#liPrep > span#prepHoursSpan > em", "prep_time_hour" => 'TEXT';
	process "li#liCook > span#cookMinsSpan > em", "cook_time_min" => 'TEXT';
	process "li#liCook > span#cookHoursSpan > em", "cook_time_hour" => 'TEXT';
	#	Image ?
};


# ----------------------------------------------------------
# scrapeRecipe - function that will call helpers to 
#	scrape all parts of the recipe and store them into
#	the recipes global ptr
#
# params	url
# return	true on success, false on error/failure logged
# ----------------------------------------------------------
sub scrape_recipe 
{
	my $url = shift;
	my $data = scrape_url( $url, $scraper_parts );

	if( $data == 0 )
	{
		$logger->error( "Error Scraping URL: $url" );
		return ( 0 => 0 );
	}

	# clean up and fill in what can be put in immediately
	my %recipe = (
		'name' 	      => '',
		'description' => '',
		'yield'       => '',
		'ingredients' => [],
		'directions'  => [],
		'prep_time'   => 0,
		'cook_time'   => 0
	);
	
	$recipe{ 'url' }	 = trim( $url );
 	$recipe{ 'name' }        = trim( ${ $data }{ 'recipe_name' } );
	$recipe{ 'description' } = trim( ${ $data }{ 'recipe_description' } );
	$recipe{ 'yield' }       = trim( ${ $data }{ 'recipe_yield' } );
	$recipe{ 'ingredients' } = ${ $data }{ 'ingredients' };
	$recipe{ 'directions' }	 = ${ $data }{ 'recipe_directions' };
	$recipe{ 'prep_time' }   += 60 * trim( ${ $data }{ 'prep_time_hour' } ) if( !is_missing_or_empty( ${ $data }{ 'prep_time_hour' } ) );
	$recipe{ 'cook_time' }   += 60 * trim( ${ $data }{ 'cook_time_hour' } ) if( !is_missing_or_empty( ${ $data }{ 'cook_time_hour' } ) );
	$recipe{ 'prep_time' }   += trim( ${ $data }{ 'prep_time_min' } ) if( !is_missing_or_empty( ${ $data }{ 'prep_time_min' } ) );
	$recipe{ 'cook_time' }   += trim( ${ $data }{ 'cook_time_min' } ) if( !is_missing_or_empty( ${ $data }{ 'cook_time_min' } ) );

	return %recipe;	
}

# ----------------------------------------------------------
# get_db_inserts - function that will take a recipe object
#	and compose the queries to insert the recipe into
#	the database
#
# params	recipe_object
# return	@array of mysql statments on success, 
#			false on error/failure logged
# ----------------------------------------------------------
sub get_db_inserts
{
	my $recipe_pointer  = shift;
	my %recipe = %{ $recipe_pointer };
	my @inserts = ();

	if( !defined \%recipe )
	{
		$logger->error( "Error Creating Inserts. Recipe dump below: \n" . Dumper( %recipe ) );
		return 0;
	}

	for( $i = 0; $i < scalar( @{ $recipe{ 'ingredients' } } ); $i++ )
	{
		my %amounts 	= ();
		my %ingredients = ();

		if( defined ${ $recipe{ 'ingredients' }[$i] }{ 'amount' } )
		{ 	
			%amounts = split_amount( ${ $recipe{ 'ingredients' }[$i] }{ 'amount' } );
		}

		print Dumper( $recipe{ 'ingredients' }[$i] );
		print Dumper( %amounts );

		#$ingredient_insert = 'INSERT INTO ingredient (id, name) VALUES (NULL, "' . $recipe{ 'ingredients' }{ 'name_desc' }[$i] . '")';
		#$measure_insert = 'INSERT INTO measure (id, name) VALUES (NULL, "' . $amount{ 'measure' } .'")';
		
		#push( @inserts, $ingredient_insert);
		#push( @inserts, $measure_insert);
	}

	# ingredient			INSERT / SELECT (include both)
	# measure    			INSERT / SELECT (include both)
	# recipe     			INSERT
	#$recipe_insert = 'INSERT INTO recipe (id, name, description, yield, url) VALUES '
	#	       . '(NULL, "' . $recipe{ 'name' } .'", "' . $recipe{ 'descrition' }
	#	       . '", "' . $recipe{ 'yield' } . '", "' . $recipe{ 'url' } . '")';
	# foreach direction in recipe
	#	recipe_direction		INSERT
	#$recipe_direction_insert = 'INSERT INTO recipe_direction (id, recipe_id, step, direction) VALUES '
	#			 . '(NULL, RECIPE_ID, INDEX, "' . $recipe{ 'directions' }[INDEX] . '")';
	# foreach ingredient in recipe
	#	recipe_ingredient	INSERT
	#$recipe_ingredient_insert = 'INSERT INTO recipe_ingredient (id, recipe_id, ingredient_id, quantity, measure_id)'
	#			  . 'VALUES (NULL, RECIPE_ID, INGREDIENT_ID, QUANTITY, MEASURE_ID)';



	return @inserts;
}

# for spliting the ingredient_amounts entries into quantity and measure
sub split_amount
{
        my $amount_str = shift;

        my %amount = (
                'quantity'     => '',
                'measure'      => ''
        );

        if( $amount_str =~ /^([0-9\/]*)\s?([0-9\/]*)\s?([(](\d+)\s(\w+)[)])?\s?(\w+)?$/ )
        {
                my $quantity = 0;

                $quantity  = fraction_to_double( $1 );

                $quantity_fraction  = fraction_to_double( $2 );

                $amount{ 'quantity' } = $quantity + $quantity_fraction;

                if( defined $3 )
                {
                       $amount{ 'alternate_quantity' }  = fraction_to_double( $4 ) if( !is_missing_or_empty( $4 ) );
                       $amount{ 'alternate_measure' } = $5 if( !is_missing_or_empty( $5 ) );
                }

                $amount{ 'measure' } = $6 if( !is_missing_or_empty( $6 ) );
        }

        return %amount;
}

# for splitting the name and description of each ingredient
sub split_name_description
{
	# figure out thet pattern after the DB analysis
}


1;
