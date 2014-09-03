#!/usr/bin/perl
use warnings;
use strict;
use File::Spec;
use POSIX qw(strftime);
use Log::Log4perl qw(get_logger);

my $stamp = strftime("%Y%m%d", localtime);

#Creates Log settings and Logger for use during runtime
Log::Log4perl::Logger::create_custom_level("ERROR", "FATAL");
my $conf = "
    log4perl.logger = INFO, screen, file
    
    log4perl.appender.file = Log::Log4perl::Appender::File
	log4perl.appender.file.Threshold = TRACE
	log4perl.appender.file.filename = logs/recipe_".$stamp.".log
	log4perl.appender.file.layout = PatternLayout
	log4perl.appender.file.layout.ConversionPattern=[%d] %-5p 	%m%n

	log4perl.appender.screen = Log::Log4perl::Appender::Screen
	log4perl.appender.screen.Threshold = ERROR
	log4perl.appender.screen.layout = PatternLayout
	log4perl.appender.screen.layout.ConversionPattern=%p - %m%n
	";
Log::Log4perl::init(\$conf);
my $logger = get_logger();

// scrape.pl text-file-with-urls output-file(optional)

if(@ARGV){
	if($#ARGV != 1){
		$logger->logexit("Invalid number of arguments\n"
						."Usage:   scrape.pl <text-file-with-url-list> [<output file>]\n" );
	}
} else {
	$logger->logexit("No arguments given, exiting now."
					."Usage:   scrape.pl <text-file-with-url-list> [<output file>]\n" );
}
$logger->miner("----- Starting Recipe Scrape -----");

my ( $url_list_file, $output-file );
our $recipe_ptr = \@recipes;
my $count = 0;

open my $url_list_fh, $url_list_file or die "Could not open $file: $!";
while( my $part = <$url_list_fh>)  {   
		    
}

close $url_list_fh;

