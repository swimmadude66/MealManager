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
my $scraper_parts = scraper {
	process "h1#itemTitle", 'recipe_name' => 'TEXT';
	process "span#lblDescription", 'recipe_description' => 'TEXT';
	process "span#lblYield", 'recipe_yield' => 'TEXT';
	process "ul.ingredient-wrap > li > label > p.fl-ing > span.ingredient-amount", 'ingredient_amounts[]' => 'TEXT';
	process "ul.ingredient-wrap > li > label > p.fl-ing > span.ingredient-name", 'ingredient_name_desc[]' => 'TEXT';
	process "div.directions > div.directLeft > ol > li", 'recipe_directions[]' => 'TEXT';
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
		return 0;
	}

	# clean up and fill in what can be put in immediately
	my %recipe = (
		'name' 		       => '',
		'description'          => '',
		'yield' 	       => '',
		'ingredient_name_desc' => [],
		'ingredient_amounts'   => [],
		'directions' 	       => []
	);

 	$recipe{ 'name' }        = trim( ${ $data }{ 'recipe_name' } );
	$recipe{ 'description' } = trim( ${ $data }{ 'recipe_description' } );
	$recipe{ 'yield' }       = trim( ${ $data }{ 'recipe_yield' } );

	$recipe{ 'ingredient_name_desc' } = ${ $data }{ 'ingredient_name_desc' };
	$recipe{ 'ingredient_amounts' }   = ${ $data }{ 'ingredient_amounts' };
	$recipe{ 'directions' }	       	  = ${ $data }{ 'recipe_directions' };

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

	for( $i = 0; $i < scalar( @{ $recipe{ 'ingredient_amounts' } } ); $i++ )
	{
		my %amount = split_amount( $recipe{ 'ingredient_amounts' }[$i] );
	}

	# ingredient			INSERT / SELECT (include both)
	# measure    			INSERT / SELECT (include both)
	# recipe     			INSERT
	# recipe_direction		INSERT
	# foreach ingredient in recipe
	#	recipe_ingredient	INSERT

	return 1;
}

# for spliting the ingredient_amounts entries into quantity and measure
sub split_amount
{
	my $amount_str = shift;

	if( !defined $amount_str )
	{
		$logger->error('Error: No Amount String provided to split_amount' );
		return 0;
	}

	my %amount = (
		'quantity' => '',
		'measure'  => ''
	);

	if( $amount_str =~ /^(\d+\/?\d*)\s?(\w+)$/ )
	{
		$amount{ 'quantity' } = $1 if( defined $1 );
		$amount{ 'measure' }  = $2 if( defined $2 );
	}
	
	return %amount;
}

# for splitting the name and description of each ingredient
sub split_name_description
{
	# figure out thet pattern after the DB analysis
}


1;
