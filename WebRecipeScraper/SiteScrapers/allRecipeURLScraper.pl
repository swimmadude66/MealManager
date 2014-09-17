#/usr/bin/perl
use warnings;
use Web::Scraper::LibXML;
use WWW::Curl::Easy;
use Data::Dumper;
use DBI;
use POSIX qw(ceil);

my $url_base = 'http://allrecipes.com/Recipes/Main.aspx?evt19=1&src=hn_parent&st=t&p34=HR_SortByTitle&vm=l&Page=';

my $scraper = scraper {
	process "div.search-tools > p.results > span.emphasized", 'recipeCount[]' => 'TEXT';
	process "div.searchResult > div.search_mostPop > div.searchRtSd > h3.resultTitle", 'recipeName[]' => 'TEXT';
	process "div.searchResult > div.search_mostPop > div.searchImg_result > a", 'recipeLink[]' => '@href';
};

my $start_time = time();

# my $database = 'mealmanager';
# my $host = 'localhost';
# my $port = '3306';
# my $dsn = "DBI:mysql:database=$database;host=$host;port=$port";
# my $dbh = DBI->connect($dsn, "root", "") or die $DBI::errstr;

# print "Connected to DB\n";

my $curl = WWW::Curl::Easy->new;
$curl->setopt( CURLOPT_HEADER, 1 );

my $count = 0;
my $pages = 0;
my $i = 0;#, $j = 0;
my $processed_recipes = 0;
my $response_body;

# First Page Scrape
$curl->setopt( CURLOPT_URL, $url_base . '1' );
$curl->setopt( CURLOPT_WRITEDATA, \$response_body );

$count = ${$res}{ 'recipeCount' }[0];
$count =~ s/,//;
$pages = ceil( $count / 20 );

#for( $i = 2; $i <= 2; $i++ )
#{
	$| = 1;

	if( !defined( ${$res}{'recipeName'} ) || !defined( ${$res}{'recipeLink'} ) )
	{
		print "Error scraping page $i : $url_base$i\n";
		next;
	}

#	$curl->setopt( CURLOPT_URL, $url_base . $i );
#	$curl->setopt( CURLOPT_WRITEDATA, \$response_body );

	my $retcode = $curl->perform;

	my $res = $parts->scrape( $response_body );
	print Dumper( $res );
#}

# Starts the actual request

