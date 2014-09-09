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
my $logger = get_logger();

sub scrape_url {
	$url = shift;
	$parts_to_scrape = shift;
	
	# scrape the data
	my $data = $parts_to_scrape->scrape(URI->new($url));
	return $data;
}



# ----------------------------------------------------------
# db_connect 	Connect to the database 
#
# params	(All Option After First Call)
#			Database Name
#			Hostname
#			Port
#			Database User
#			Database User Password
# return	Database Handle
# ----------------------------------------------------------
sub db_connect 
{
	if($database_handle == undef)
	{
		my $db_name = shift;
		my $host = shift;
		my $port = shift;
		my $db_user = shift;
		my $db_user_pass = shift;

		my $db_connection_string = "DBI:mysql:database=$database;host=$host;port=$port";
		$database_handle = DBI->connect($db_connection_string, "root", "");
	}

	return $database_handle;
}

# ----------------------------------------------------------
# db_disconnect 	Disconnect from the database 
# ----------------------------------------------------------
sub db_disconnect 
{
	if($database_handle != undef)
	{
		$database_handle->disconnect;
		$database_handle = undef;
	}
}
