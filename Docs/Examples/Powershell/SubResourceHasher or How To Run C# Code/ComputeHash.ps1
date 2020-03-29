$assemblies=(
	"System",
	"System.Net.Http",
	"System.Security",
	"System.Text.Encoding"
)


$source=@"
using System;
namespace SubResourceHasher
{
	public static class ComputeHash{
		
		public static void Main(){
		
			Console.WriteLine("What is the url you want to compute the hash for?");
			var url = Console.ReadLine();
		
			var client = new System.Net.Http.HttpClient();
			var htmlContent = client.GetStringAsync(url).Result;
			
			using (var sha512Hasher = System.Security.Cryptography.SHA512Managed.Create())  
			{  
				var result = sha512Hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(htmlContent));
					
				Console.WriteLine("sha512-" + Convert.ToBase64String(result));
				Console.WriteLine("Press Any Key To Exit");
				Console.ReadKey();
			}  
		}
	}
}
"@

Add-Type -ReferencedAssemblies $assemblies -TypeDefinition $source -Language CSharp
[SubResourceHasher.ComputeHash]::Main()