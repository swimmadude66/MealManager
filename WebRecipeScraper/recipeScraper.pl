#!/usr/bin/perl
use warnings;
use strict;
use File::Spec;
use POSIX qw(strftime);
use Log::Log4perl qw(get_logger);

# scrape.pl text-file-with-urls output-file(optional)
if( !@ARGV )
{
	print( "No arguments given, exiting now.\n"
		  ."Usage:   scrape.pl <text-file-with-url-list> [<output file>]\n" );
	exit();
}

my ( $url_list_file, $output-file );
switch( $#ARGV )
{
	case 0: $
}

my $stamp = strftime( "%Y%m%d", localtime );
my $database = 'mealmanager-test';
my $host = 'localhost';
my $port = '3306';
my $user = 'root';
my $pass = '';

my $logger = get_logger();

$logger->( "----- Starting Recipe Scrape -----" );


our $recipe_ptr = \@recipes;
my $count = 0;

open my $url_list_fh, $url_list_file or die "Could not open $file: $!";
while( my $line = <$url_list_fh> )  
{   

	# regex text for the url to match to the correct scraper script
	# scrape and get the recipe
	# add recipe to the recipe array

	$count++;
}

# dump JSON array to file
# insert into DB (more details TBD)

close $url_list_fh;

