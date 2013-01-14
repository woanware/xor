using CommandLine;
using CommandLine.Text;
using System;

namespace xor
{
    /// <summary>
    /// Internal class used for the command line parsing
    /// </summary>
    internal class Options : CommandLineOptionsBase
    {
        [Option("i", "input", Required = true, DefaultValue = "", HelpText = "The input file")]
        public string Input { get; set; }

        [Option("m", "mode", Required = true, DefaultValue = "", HelpText = "The xor mode e.g. one byte (1), two byte (2), four byte (4), rolling (r)")]
        public string Mode { get; set; }

        [Option("k", "key", Required = true, DefaultValue = "", HelpText = "The XOR key")]
        public string Key { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Copyright = new CopyrightInfo("woanware", 2013),
                AdditionalNewLineAfterOption = false,
                AddDashesToOption = true
            };

            this.HandleParsingErrorsInHelp(help);

            help.AddPreOptionsLine("Usage: xor -i \"input\" -m \"4\" -k \"3D4G1A8F\"");
            help.AddOptions(this);

            return help;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="help"></param>
        private void HandleParsingErrorsInHelp(HelpText help)
        {
            if (this.LastPostParsingState.Errors.Count > 0)
            {
                var errors = help.RenderParsingErrorsText(this, 2); // indent with two spaces
                if (!string.IsNullOrEmpty(errors))
                {
                    help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                    help.AddPreOptionsLine(errors);
                }
            }
        }
    }
}
