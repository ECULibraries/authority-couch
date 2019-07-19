﻿using System;
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

        public GuideViewModel GetResourceAuthorities(int resourceId)
        {
            var gvm = new GuideViewModel();

            var agentSql = Sql.Builder
                .Select("linked_agents_rlshp.agent_person_id, linked_agents_rlshp.agent_family_id, linked_agents_rlshp.agent_corporate_entity_id, " +
                        "linked_agents_rlshp.role_id, linked_agents_rlshp.relator_id, name_person.sort_name AS person_name, " +
                        "name_family.sort_name AS family_name, name_corporate_entity.sort_name AS corp_name")
                .From("linked_agents_rlshp")
                .LeftJoin("agent_person").On("agent_person.id = linked_agents_rlshp.agent_person_id")
                .LeftJoin("name_person").On("name_person.agent_person_id = linked_agents_rlshp.agent_person_id")
                .LeftJoin("agent_family").On("agent_family.id = linked_agents_rlshp.agent_family_id")
                .LeftJoin("name_family").On("name_family.agent_family_id = linked_agents_rlshp.agent_family_id")
                .LeftJoin("agent_corporate_entity").On("agent_corporate_entity.id = linked_agents_rlshp.agent_corporate_entity_id")
                .LeftJoin("name_corporate_entity").On("name_corporate_entity.agent_corporate_entity_id = linked_agents_rlshp.agent_corporate_entity_id")
                .Where("resource_id = @0", resourceId)
                .OrderBy("person_name, family_name, corp_name");

            var subjectSql = Sql.Builder
                .Select("subject.id, subject.title AS subject, enumeration_value.value AS type")
                .From("subject_rlshp")
                .LeftJoin("subject").On("subject.id = subject_rlshp.subject_id")
                .LeftJoin("subject_term").On("subject_term.subject_id = subject_rlshp.subject_id")
                .LeftJoin("term").On("term.id = subject_term.id")
                .LeftJoin("enumeration_value").On("enumeration_value.id = term.term_type_id")
                .Where("resource_id = @0", resourceId);

            using (var db = Connection)
            {
                gvm.AsAgents = db.Fetch<AgentGroup>(agentSql);
                gvm.AsSubjects = db.Fetch<SubjectGroup>(subjectSql);
            }

            return gvm;
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