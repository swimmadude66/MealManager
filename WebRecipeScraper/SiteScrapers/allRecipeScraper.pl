#!/usr/bin/perl
# allRecipeScraper.pl
use 'common.pl'

my $i;
my $logger = get_logger();
our $recipe_ptr;

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
sub scrapeRecipe {
	my $url = shift;
	my $data = scrapeURL($url, $scraper_parts);

	# clean up and fill in what can be put in immediately
	my @recipe = array(
		'name' => '',
		'description' => '',
		'yield' => '',
		'ingredients' => array(),
		'directions' => array()
	);

	# parse and split ingredient amounts
	# parse and split ingredient description + name

}

# subs to create
#	amount_split		for spliting the ingredient_amounts entries into quantity and measure
#	name_desc_split		for splitting the name and description of each ingredient