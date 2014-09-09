#!/usr/bin/perl
# common.pl
use warnings;
use File::Spec;
use URI;
use Web::Scraper;
use Data::Dumper;
use Log::Log4perl qw(get_logger);
use DBI;

my $database_handle;
my $i;
my $logger;

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
	$url 			 = shift;
	$parts_to_scrape = shift;

	if( !defined( $url ) || !defined( $parts_to_scrape ) )
	{
		$logger->error( 'Invalid scrape_url call ( No Parameters Supplied )' );
		exit();
	}
	
	# scrape the data
	my $data = $parts_to_scrape->scrape( URI->new( $url ) );
	return $data;
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
		my $host 		 = shift;
		my $port 		 = shift;
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
