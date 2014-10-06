#!/usr/bin/perl
use strict;
use warnings;
require 'SiteScrapers/allRecipeScraper.pl';

# scrape.pl text-file-with-urls output-file(optional)
if( !@ARGV )
{
	print( "No arguments given, exiting now.\n"
		  ."Usage:   scrape.pl <text-file-with-url-list> [<output file>]\n" );
	exit();
}

my ( $url_list_file, $output_file );
#my $stamp = strftime( "%Y%m%d", localtime );
#my $database = 'mealmanager';
#my $host = 'meal-manager-cs-4911.chplv1mpw4j4.us-west-2.rds.amazonaws.com';
#my $port = '3306';
#my $user = 'recipesrc';
#my $pass = 'giveMeMeals2014';

if( scalar( @ARGV ) == 1 )
{
	# no output file supplied
	$url_list_file = $ARGV[0];
	$output_file   = 'output.log';
} 
elsif( scalar( @ARGV ) == 2 )
{
	# output file supllied
	$url_list_file = $ARGV[0];
	$output_file   = $ARGV[1];
} 
else
{
	print( "No arguments given, exiting now.\n"
		  ."Usage:   scrape.pl <text-file-with-url-list> [<output file>]\n" );
	exit();
}

log_init();

my $logger = get_logger();
my $start_time = time();

$logger->info( "----- Starting Recipe Scrape -----" );

#our $recipe_ptr = \@recipes;
my $count = 0;
my $i = 0;

open my $url_list_fh, $url_list_file or die "Could not open $url_list_file: $!";
open INGR, '>allrecipes.csv';

while( my $line = <$url_list_fh> )  
{   
	my $name = '';
	my $url = '';
	
	if( $line =~ /^(.*), (http:[^,]*), .*$/ )
	{
		$name = $1 if( defined $1 );
		$url = $2 if( defined $2 );
	}
	else
	{
		$logger->error( "Invalid URL Listing on line $% of $url_list_file" );
		next;
	}

	# regex text for the url to match to the correct scraper script
	# scrape and get the recipe
	my %recipe = scrape_recipe( $url );
	next if( defined $recipe{ 0 } );

	print INGR $name . "||URL||" . $url . "\n";
	print INGR $name . "||Description||" . $recipe{ 'description' } . "\n";
	print INGR $name . "||Yield||" . $recipe{ 'yield' } . "\n";
	print INGR $name . "||Prep Time||" . $recipe{ 'prep_time' } . "\n";
	print INGR $name . "||Cook Time||" . $recipe{ 'cook_time' } . "\n";

	foreach my $ingredient ( @{ $recipe{ 'ingredients' } } )
	{
		print INGR $name . "||Ingredient||";
		print INGR ${ $ingredient }{ 'amount' } . "||";
		print INGR ${ $ingredient }{ 'name_desc' } . "\n";
	}

	my $i = 0;
	foreach my $direction( @{ $recipe{ 'directions' } } )
	{
		print INGR $name . "||Direction||$i||" . $direction . "\n";
		$i++;
	}

	foreach my $nutrient_ptr ( @{ $recipe{ 'nutrition' } } )
	{	
		my %nutrient = %{ $nutrient_ptr };
		print INGR $name . "||Nutrition||" . $nutrient{ 'name' } . "||" 
	 	    . $nutrient{ 'unit' } . "||" . $nutrient{ 'per_of_daily' } . "\n";
	}

	print INGR "\n";

	$count++;

	if( $count % 50 == 0 )
	{
		my $curr_exec = time() - $start_time;
		print "$count recipes scraped ($curr_exec seconds) \n"; 
	}
}

close INGR;
close $url_list_fh;

