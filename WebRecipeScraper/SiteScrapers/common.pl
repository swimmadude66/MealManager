#!/usr/bin/perl
# common.pl
use warnings;
use utf8;
binmode( STDOUT, ': utf8' );

use File::Spec;
use Web::Scraper::LibXML;
use WWW::Curl::Easy;
use Data::Dumper;
use Log::Log4perl qw(get_logger);
use DBI;
use String::Util qw(trim);
use POSIX qw(ceil strftime);

my $database_handle;
my $i;
my $logger;
#our @recipes;

# ----------------------------------------------------------
# log_init		Sets up logging for this application
#
# params 		Path to store log file (Optional)
# ----------------------------------------------------------
sub log_init
{
	my $stamp = strftime( "%Y%m%d", localtime );
	my $logfile_path = shift;
	if( !defined( $logfile_path ) )
	{
		$logfile_path = "recipe_scrape";
	}

	#Creates Log settings and Logger for use during runtime
	Log::Log4perl::Logger::create_custom_level( "ERROR", "FATAL" );
	my $conf = "
	    log4perl.logger = INFO, screen, file
	    
	    log4perl.appender.file = Log::Log4perl::Appender::File
		log4perl.appender.file.Threshold = DEBUG
		log4perl.appender.file.filename = logs/".$logfile_path."_".$stamp.".log
		log4perl.appender.file.layout = PatternLayout
		log4perl.appender.file.layout.ConversionPattern=[%d] %-5p 	%m%n

		log4perl.appender.screen = Log::Log4perl::Appender::Screen
		log4perl.appender.screen.Threshold = ERROR
		log4perl.appender.screen.layout = PatternLayout
		log4perl.appender.screen.layout.ConversionPattern=%p - %m%n
		";
	Log::Log4perl::init( \$conf );

	$logger = get_logger();
}

# ----------------------------------------------------------
# scrape_url	Scrapes the configured part(s) from the URL
#
# params		URL to Scrape
#				Scrapper Object that outlines info to be collected
# return		Results from scrape (Hash Pointer)
# ----------------------------------------------------------
sub scrape_url 
{
	my $url	    = shift;
	my $scraper = shift;

	if( !defined( $url ) || !defined( $scraper ) )
	{
		$logger->error( 'Invalid scrape_url call ( No Parameters Supplied )' );
		return 0;
	}
	
	my $curl_handle  = WWW::Curl::Easy->new();
	my $page_content = '';
	my $scraped_data = '';
	
	$curl_handle->setopt( CURLOPT_HEADER, 0 );
	$curl_handle->setopt( CURLOPT_URL, $url );
	$curl_handle->setopt( CURLOPT_WRITEDATA, \$page_content );
	
	my $curl_status = $curl_handle->perform;
	if( $curl_status != CURLE_OK )
	{
		$logger->error( "Error Scraping $url - " . $curl_handle->errbuf );
		return 0;	
	}

	$scraped_data = $scraper->scrape( $page_content );

	undef $curl_handle;
	undef $page_content;

	return $scraped_data;
}

# ----------------------------------------------------------
# db_connect 	Connect to the database 
#
# params		(All Option After First Call)
#				Database Name
#				Hostname
#				Port
#				Database User
#				Database User Password
# return		Database Handle
# ----------------------------------------------------------
sub db_connect 
{
	if( !defined( $database_handle ) )
	{
		my $db_name 	 = shift;
		my $host 	 = shift;
		my $port 	 = shift;
		my $db_user 	 = shift;
		my $db_user_pass = shift;

		if( !defined( $db_name ) || !defined( $host ) || !defined( $port ) || !defined( $db_user ) || !defined( $db_user_pass ) )
		{
			$logger->error( 'Cannot connect to DB (error in stdout)');
			exit();
		}

		my $db_connection_string = "DBI:mysql:database=$database;host=$host;port=$port";
		$database_handle = DBI->connect( $db_connection_string, "root", "" ) or die $DBI::errstr;
	}

	return $database_handle;
}

# ----------------------------------------------------------
# db_disconnect 	Disconnect from the database 
# ----------------------------------------------------------
sub db_disconnect 
{
	if( !defined($database_handle ) )
	{
		$database_handle->disconnect;
		$database_handle = undef;
	}
}

# ----------------------------------------------------------
# escape		Escape quotes from strings
# 
# params		String
# return		Escaped String
# ----------------------------------------------------------
sub escape
{
	return '' if( is_missing_or_empty( $_[0] ) );
	my $string = $_[0];
	$string = trim( $string );

	return quotemeta $string;
}

sub fraction_to_double
{
        return 0 if( is_missing_or_empty( $_[0] ) );
 		
 		my $num = $_[0];
 		my $value = 0;

        return $num if( $num !~ /[\/]/ );

        if( $num =~ /(\d+)[\/](\d+)/ )
        {
                $value = $1 if( !is_missing_or_empty( $1 ) );

                if( !is_missing_or_empty( $2 ) )
                {
                        $value = $value / $2;
                }
        }

        return $value;
}

sub is_missing_or_empty
{
	return 1 if( !defined $_[0] );
	return 1 if( $_[0] eq '' );
	return 0;
}

sub trim 
{ 
	my $s = shift; 
	$s =~ s/^\s+|\s+$//g; 
	return $s;
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









1;
