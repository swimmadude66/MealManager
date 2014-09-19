#!/usr/bin/perl
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
#my $database = 'mealmanager-test';
#my $host = 'localhost';
#my $port = '3306';
#my $user = 'root';
#my $pass = '';

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

$logger->info( "----- Starting Recipe Scrape -----" );


#our $recipe_ptr = \@recipes;
my $count = 0;
my $i = 0;

open my $url_list_fh, $url_list_file or die "Could not open $url_list_file: $!";
while( my $line = <$url_list_fh> )  
{   
	my $name = '', $url = '';

	if( $line =~ /^([^,]*), ([^,]*), .*$/ )
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
	scrape_recipe( $url );
	exit();	

	# add recipe to the recipe array

	$count++;
}

# dump JSON array to file
# insert into DB (more details TBD)

close $url_list_fh;

