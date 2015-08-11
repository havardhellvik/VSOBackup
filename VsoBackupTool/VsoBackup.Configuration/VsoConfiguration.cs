using System.Configuration;

namespace VsoBackup.Configuration
{
    internal class VsoConfiguration : ConfigurationSection, IVsoConfiguration
    {
        [ConfigurationProperty("ApiUsername", IsRequired = true)]
        public string ApiUsername
        {
            get
            {
                return this["ApiUsername"].ToString();
            }
        }

        [ConfigurationProperty("ApiPassword", IsRequired = true)]
        public string ApiPassword
        {
            get
            {
                return this["ApiPassword"].ToString();
            }
        }

        [ConfigurationProperty("AllRepositoriesUrl", IsRequired = true)]
        public string AllRepositoriesUrl
        {
            get
            {
                return this["AllRepositoriesUrl"].ToString();
            }
        }

        [ConfigurationProperty("AllBranches", IsRequired = false)]
        public bool AllBranches
        {
            get { return this["AllBranches"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
    }
}
