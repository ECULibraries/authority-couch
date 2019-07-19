using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using AuthorityCouch.Helpers;
using AuthorityCouch.Models;
using AuthorityCouch.Models.Import;
using NPoco;

namespace AuthorityCouch.Repositories
{
    public class ArchivesSpaceRepo
    {
        private readonly string _connectionString;

        public ArchivesSpaceRepo()
        {
            _connectionString = "AsConnString";
        }

        public IDatabase Connection => new Database(_connectionString);

        public List<AsResource> GetArchivesSpaceResources()
        {
            var cachedResources = new List<AsResource>();

            if (CacheHelper.Get("archivesspace_cache", out cachedResources)) return cachedResources;

            using (var db = Connection)
            {
                cachedResources = db.Fetch<AsResource>("SELECT id, title, ead_id, repo_id FROM resource WHERE publish = 1 ORDER BY title;");
                CacheHelper.Add(cachedResources, "archivesspace_cache", DateTime.Now.AddHours(24));
            }

            return cachedResources;
        }

        public void CreateSubjectRelationship()
        {
            var meeting = new Meeting();

            using (var db = Connection)
            {
                foreach (var sub in meeting.NewAsRels)
                {
                    var subjectRlshp = new AsSubjectRelationship
                    {
                        resource_id = int.Parse(sub[0]),
                        subject_id = int.Parse(sub[1]),
                        aspace_relationship_position = 0,
                        system_mtime = DateTime.Now,
                        user_mtime = DateTime.Now,
                        suppressed = 0
                    };

                    db.Insert(subjectRlshp);
                }
            }
        }

        public void SaveAsSubjects(Meeting meeting)
        {
            using (var db = Connection)
            {
                foreach (var sub in meeting.NewAs)
                {
                    //add
                    var nterm = new AsTerm
                    {
                        lock_version = 0,
                        json_schema_version = 1,
                        vocab_id = 1,
                        term = sub.Trim(),
                        term_type_id = 1276,
                        created_by = "admin",
                        last_modified_by = "admin",
                        create_time = DateTime.Now,
                        system_mtime = DateTime.Now,
                        user_mtime = DateTime.Now
                    };

                    db.Insert(nterm);

                    // new subject if new term
                    var newSub = GetNewSubject(sub.Trim());
                    db.Insert(newSub);
                    var subjectTerm = new AsSubjectTerm {subject_id = newSub.id, term_id = nterm.id};
                    db.Insert(subjectTerm);
                }
            }
        }

        private AsSubject GetNewSubject(string value)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            var temp = sha.ComputeHash(Encoding.UTF8.GetBytes(value + "1276"));
            var sb = new StringBuilder();
            foreach (byte t in temp)
            {
                sb.Append(t.ToString("x2"));
            }

            var nsubject = new AsSubject
            {
                lock_version = 0,
                json_schema_version = 1,
                vocab_id = 1,
                title = value,
                terms_sha1 = sb.ToString(),
                source_id = 361,//360 lc,
                created_by = "admin",
                last_modified_by = "admin",
                create_time = DateTime.Now,
                system_mtime = DateTime.Now,
                user_mtime = DateTime.Now
            };

            return nsubject;
        }
    }
}