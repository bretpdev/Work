using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitCloner
{
    class Cloner
    {
        const string OrganizationName = "UHEAA";
        const string GithubUrl = @"https://github.uheaa.org/UHEAA";
        const double BytesInAGigabyte = 1073741824d;
        public ProgressLog ProgressLog { get; set; }
        public string ApiKey { get; set; }
        public string CloneLocation { get; set; }
        public bool CleanFirst { get; set; }
        public bool IncludeBranches { get; set; }
        public Cloner(ProgressLog progressLog, string apiKey, string cloneLocation, bool cleanFirst, bool includeBranches)
        {
            ProgressLog = progressLog;
            ApiKey = apiKey;
            CloneLocation = cloneLocation;
            CleanFirst = cleanFirst;
            IncludeBranches = includeBranches;
        }

        public void Clone()
        {
            try
            {
                DateTime start = DateTime.Now;
                long totalSize = 0;
                if (!Directory.Exists(CloneLocation))
                {
                    Log($"Location '{CloneLocation}' does not exist.");
                    return;
                }
                if (CleanFirst)
                {
                    Status($"Cleaning {CloneLocation}");
                    foreach (var file in Directory.GetFiles(CloneLocation))
                        File.Delete(file);
                    foreach (var directory in Directory.GetDirectories(CloneLocation))
                        Directory.Delete(directory, true);
                    Log($"Finished cleaning {CloneLocation}");
                }
                Status("Beginning Git Clone");
                Status("Gathering Repository Information");
                var client = new GitHubClient(new ProductHeaderValue(OrganizationName), new Uri(GithubUrl)); //start at the UHEAA org level
                client.Credentials = new Credentials(ApiKey);

                var repositories = GetRepositories(client);
                int repositoryMax = repositories.Count();
                Log($"Found {repositoryMax} repositories to clone.");
                int repositoryCount = 0;
                foreach (var repo in repositories)
                {
                    repositoryCount++;
                    Status($"Cloning {repo.Name} ({repositoryCount} / {repositoryMax})");
                    CloneRepo(client, repo);
                    totalSize += repo.Size;
                }
                Status("Git Clone Complete");
                //double totalGigabytes = totalSize / BytesInAGigabyte / 8;
                //Log($"Elapsed Time: {(int)(DateTime.Now - start).TotalMinutes}m, Total Size: {totalGigabytes}gb");
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException is AuthorizationException)
                    Status($"Unable to authenticate token.  ({ae.InnerException.Message})");
                else
                {
                    Status($"Error ({ae.InnerExceptions.Count}):");
                    foreach (var exception in ae.InnerExceptions)
                        Log(exception.Message);
                }
            }
            catch (Exception ex)
            {
                Status("Error: " + ex.ToString());
            }
            finally
            {
                ProgressLog.Log("----------------------------------------", false);
            }
        }

        private void CloneRepo(GitHubClient client, Repository repo)
        {
            var zippedTask = client.Repository.Content.GetArchive(repo.Id, ArchiveFormat.Zipball);
            zippedTask.Wait();
            var directoryName = Path.Combine(CloneLocation, repo.Name);
            string initPath = "";
            using (var inputStream = new MemoryStream(zippedTask.Result))
            using (var archive = new ZipArchive(inputStream))
            {
                initPath = Path.Combine(directoryName, archive.Entries[0].FullName.Replace("/", "\\"));
                foreach (var x in archive.Entries)
                {
                    //string path = Path.GetFullPath(directoryName);
                    string path = Path.Combine(directoryName, x.FullName.Replace("/", "\\"));

                    if (x.Name != "")
                    {
                        FileInfo fi = new FileInfo(path);
                        if (!fi.Exists || fi.Length != x.Length)
                            x.ExtractToFile(path, true);
                    }
                    else if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
            }

            Copy(initPath, directoryName);
            Directory.Delete(initPath, true);
            //archive.ExtractToDirectory(directoryName);
        }

        private void Copy(string srcDir, string tgtDir)
        {
            if (!Directory.Exists(tgtDir))
                Directory.CreateDirectory(tgtDir);

            foreach (var file in Directory.GetFiles(srcDir))
                File.Copy(file, Path.Combine(tgtDir, Path.GetFileName(file)));

            foreach (var directory in Directory.GetDirectories(srcDir))
                Copy(directory, Path.Combine(tgtDir, Path.GetFileName(directory)));
        }

        private IEnumerable<Repository> GetRepositories(GitHubClient client)
        {
            var repos = client.Repository.GetAllForOrg(OrganizationName);
            repos.Wait();
            foreach (var result in repos.Result)
                if (!result.Private && result.Size != 0) //filter private repositories and repositories with no commits
                    yield return result;
        }

        private void Log(string message)
        {
            ProgressLog.Log(message);
        }

        private void Status(string status)
        {
            ProgressLog.Status(status);
        }
    }
}
