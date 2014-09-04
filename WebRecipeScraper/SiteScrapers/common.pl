#!/usr/bin/perl
# common.pl
use warnings;
use File::Spec;
use URI;
use Web::Scraper;
use Data::Dumper;
use Log::Log4perl qw(get_logger);

my $i;
my $logger = get_logger();

sub scrapeURL {
	$url = shift;
	$parts_to_scrape = shift;
	
	# scrape the data
	my $data = $parts_to_scrape->scrape(URI->new($url));
	return $data;
}