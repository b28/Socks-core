using CommandLine;

namespace CustomServer.ConnectionAcceptor.Server
{
    class Options
    {


        [Option("external-port", HelpText = "External port to listen.", Required = true)]
        public int ExternalPort { get; set; }

        [Option("internal-ip", HelpText = "Internal IP address  to listen.", Required = true)]
        public string InternalIp { get; set; }

        [Option("external-ip", HelpText = "External IP address to listen.", Required = true)]
        public string ExternalIp { get; set; }

        [Option("admin-username", HelpText = "Admin login name.", Required = true)]
        public string AdminName { get; set; }

        [Option("admin-password", HelpText = "Password to admin account.", Required = true)]
        public string AdminPassword { get; set; }


    }
}