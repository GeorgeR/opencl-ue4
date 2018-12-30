using System.IO;

namespace UnrealBuildTool.Rules
{
    public class OpenCL : ModuleRules
    {
        private string ModulePath
        {
            get { return ModuleDirectory; }
        }

        private string ThirdPartyPath
        {
            get { return Path.GetFullPath(Path.Combine(ModuleDirectory, "../../ThirdParty/")); }
        }

        public OpenCL(ReadOnlyTargetRules Target) : base(Target)
        {
            PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

            var IncludePath = Path.Combine(ThirdPartyPath, "OpenCL", "Include");

            if(!Directory.Exists(IncludePath))
                throw new DirectoryNotFoundException(IncludePath);

            if (Target.Version.MinorVersion <= 19)
            {
                PublicIncludePaths.AddRange(
                    new string[] {
                        "OpenCL/Public",
                        IncludePath
                });
            }

            PublicIncludePaths.AddRange(
                new string[] {
                    IncludePath
                });

            PublicDependencyModuleNames.AddRange(
                new string[]
                {
                    "Core",
                    "CoreUObject",
                    "Engine",
                    "RenderCore",
                    "ShaderCore",
                    "RHI"
                });

            string PlatformString = (Target.Platform == UnrealTargetPlatform.Win64) ? "Win64" : "Win32";
            string OpenCLLibrariesPath = Path.Combine(ThirdPartyPath, "OpenCL", "Lib");

            string NvidiaLibrariesPath = Path.Combine(OpenCLLibrariesPath, "NVIDIA", PlatformString);
            string IntelLibrariesPath = Path.Combine(OpenCLLibrariesPath, "Intel", PlatformString);
            string AmdLibrariesPath = Path.Combine(OpenCLLibrariesPath, "AMD", PlatformString);

            if (Target.Platform == UnrealTargetPlatform.Win64 || Target.Platform == UnrealTargetPlatform.Win32)
            {
                PublicAdditionalLibraries.Add(Path.Combine(NvidiaLibrariesPath, "OpenCL.lib"));
                PublicAdditionalLibraries.Add(Path.Combine(IntelLibrariesPath, "OpenCL.lib"));
                PublicAdditionalLibraries.Add(Path.Combine(AmdLibrariesPath, "OpenCL.lib"));
            }
            else if (Target.Platform == UnrealTargetPlatform.Mac)
            {
                PublicAdditionalFrameworks.Add(new UEBuildFramework("OpenCL"));
            }

            // TODO: add linux support
        }
    }
}
