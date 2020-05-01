using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;
using System.Linq;
using AuthorityCouch.Models;
using AuthorityCouch.Models.Import;
using AuthorityCouch.Repositories;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace AuthorityCouch.Controllers
{
    public class BaseController : Controller
    {
        public ArchivesSpaceRepo AsRepo;
        private RestClient _client => new RestClient(ConfigurationManager.AppSettings["CouchBase"])
        {
            Authenticator = new HttpBasicAuthenticator(ConfigurationManager.AppSettings["CouchAdmin"],
                ConfigurationManager.AppSettings["CouchPass"])
        };


    public BaseController()
        {
            AsRepo = new ArchivesSpaceRepo();
        }

        public CouchResponse GetUuids(int count)
        {
            var resource = count <= 1 ? "_uuids" : $"_uuids?count={count}";
            var request = new RestRequest(resource, Method.GET, DataFormat.Json);
            return JsonConvert.DeserializeObject<CouchResponse>(_client.Execute(request).Content);
        }

        public SearchViewModel SearchNamePrefix(SearchViewModel svm)
        {
            var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{ \"selector\": {\"authoritativeLabel\": {\"$regex\": \"^" + svm.Term.Replace("(", @"\\(").Replace(")", @"\\)") + "\"} }, \"sort\": [{\"authoritativeLabel\": \"asc\"}], \"limit\": 1000 }", ParameterType.RequestBody);

            var test = _client.Execute(request).Content;
            svm.Results = JsonConvert.DeserializeObject<CouchDocs>(test);
            return svm;
        }

        public SearchViewModel SearchSubjectPrefix(SearchViewModel svm)
        {
            var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{ \"selector\": {\"authoritativeLabel\": {\"$regex\": \"^" + svm.Term.Replace("(",@"\\(").Replace(")", @"\\)") + "\"} }, \"sort\": [{\"authoritativeLabel\": \"asc\"}], \"limit\": 1000 }", ParameterType.RequestBody);

            var test = _client.Execute(request).Content;
            svm.Results = JsonConvert.DeserializeObject<CouchDocs>(test);
            return svm;
        }

        public SearchViewModel SearchNameByLabel(SearchViewModel svm)
        {
            var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{\"selector\":{\"authoritativeLabel\": \"" + svm.Term + "\" } }", ParameterType.RequestBody);
            svm.Results = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
            return svm;
        }

        public SearchViewModel SearchSubjectByLabel(SearchViewModel svm)
        {
            var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{\"selector\":{\"authoritativeLabel\": \"" + svm.Term + "\" } }", ParameterType.RequestBody);
            svm.Results = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
            return svm;
        }

        public SearchViewModel SearchNameByAsUri(string uri)
        {
            var svm = new SearchViewModel();
            var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{\"selector\": {\"$or\": [ {\"creator\":{\"$elemMatch\": {\"$eq\": \"" + uri + "\"} }}, " +
                                                     "{\"source\": {\"$elemMatch\": {\"$eq\": \"" + uri + "\"} }}] }, \"limit\": 100 }", ParameterType.RequestBody);
            svm.Results = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
            return svm;
        }

        public SearchViewModel SearchNameByDcUri(string uri)
        {
            var svm = new SearchViewModel();
            var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{\"selector\": {\"dcName\": {\"$elemMatch\":{\"uri\": \"" + uri + "\"} }}, \"limit\": 100 }", ParameterType.RequestBody);
            svm.Results = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
            return svm;
        }

        public SearchViewModel SearchSubjectByAsUri(string uri)
        {
            var svm = new SearchViewModel();
            var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{\"selector\": {\"$or\": [ {\"topic\":{\"$elemMatch\": {\"$eq\": \"" + uri + "\"} }}, " +
                                                     "{\"geographic\": {\"$elemMatch\": {\"$eq\": \"" + uri + "\"} }}, " +
                                                     "{\"personalNameSubject\": {\"$elemMatch\": {\"$eq\": \"" + uri + "\"} }}, " +
                                                     "{\"familyNameSubject\": {\"$elemMatch\": {\"$eq\": \"" + uri + "\"} }}, " +
                                                     "{\"corporateNameSubject\": {\"$elemMatch\": {\"$eq\": \"" + uri + "\"} }}, " +
                                                     "{\"meeting\": {\"$elemMatch\": {\"$eq\": \"" + uri + "\"} }}, " +
                                                     "{\"uniformTitle\": {\"$elemMatch\": {\"$eq\": \"" + uri + "\"} }}] }, \"limit\": 100, \"sort\": [{\"authoritativeLabel\": \"asc\"}] }", ParameterType.RequestBody);
            svm.Results = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
            svm.Results.Docs.OrderBy(x => x.topic).OrderBy(x => x.geographic);
            return svm;
        }

        public SearchViewModel SearchSubjectByDcUri(string uri)
        {
            var svm = new SearchViewModel();
            var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{\"selector\": {\"dcSubject\": {\"$elemMatch\":{\"uri\": \"" + uri + "\"} }}, \"limit\": 100 }", ParameterType.RequestBody);
            svm.Results = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
            return svm;
        }

        public void DeleteNameDoc(string id)
        {
            var doc = GetNameDocByUuid(id);
            var request = new RestRequest($"name_authority/{doc._id}?rev={doc._rev}", Method.DELETE, DataFormat.Json);
            _client.Execute(request);
        }

        public void DeleteSubjectDoc(string id)
        {
            var doc = GetSubjectDocByUuid(id);
            var request = new RestRequest($"subject_authority/{doc._id}?rev={doc._rev}", Method.DELETE, DataFormat.Json);
            _client.Execute(request);
        }

        public List<Row> GetNameAllDocs()
        {
            var request = new RestRequest($"name_authority/_all_docs?include_docs=true", Method.GET, DataFormat.Json);
            var doc = JsonConvert.DeserializeObject<AllDocs>(_client.Execute(request).Content);
            return doc.rows;
        }

        public CouchDocs GetDcNameAllDocs()
        {
            var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{\"selector\": {\"dcName\": {\"$gt\":null}}}", ParameterType.RequestBody);
            return JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
        }

        public List<Row> GetSubjectAllDocs()
        {
            var request = new RestRequest($"subject_authority/_all_docs?include_docs=true", Method.GET, DataFormat.Json);
            var doc = JsonConvert.DeserializeObject<AllDocs>(_client.Execute(request).Content);
            return doc.rows;
        }

        public CouchDocs GetDcSubjectAllDocs()
        {
            var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
            request.AddParameter("application/json", "{\"selector\": {\"dcSubject\": {\"$gt\":null}}}", ParameterType.RequestBody);
            return JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
        }

        public Doc GetNameDocByUuid(string uuid)
        {
            var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
            request.AddJsonBody(new { selector = new { _id = uuid } });
            var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
            return doc.Docs[0];
        }

        public Doc GetSubjectDocByUuid(string uuid)
        {
            var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
            request.AddJsonBody(new { selector = new { _id = uuid } });
            var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);
            return doc.Docs[0];
        }

        public void SaveNameDoc(Doc doc)
        {
            var request = new RestRequest($"name_authority/{doc._id}/", Method.PUT, DataFormat.Json);
            var json = JsonConvert.SerializeObject(doc, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            request.AddJsonBody(json);
            _client.Execute(request);
        }

        public void SaveSubjectDoc(Doc doc)
        {
            var request = new RestRequest($"subject_authority/{doc._id}/", Method.PUT, DataFormat.Json);
            var json = JsonConvert.SerializeObject(doc, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            request.AddJsonBody(json);
            _client.Execute(request);
        }


        public void AddSubstrings()
        {
            var subdata = new SubstringData();
            foreach(var item in subdata.Data2)
            {
                var doc = GetSubjectDocByUuid(item[0]);

                doc.substrings = new List<Substring>();
                doc.substrings.Add(new Substring(item[1], null));
                doc.substrings.Add(new Substring(item[2], null));
                //doc.substrings.Add(new Substring(item[3], null));
                //doc.substrings.Add(new Substring(item[4], null));
                //doc.substrings.Add(new Substring(item[5], null));
                //doc.substrings.Add(new Substring(item[6], null));

                var request = new RestRequest($"subject_authority/{doc._id}/", Method.PUT, DataFormat.Json);
                var json = JsonConvert.SerializeObject(doc, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }
        }

        public void AddAuths()
        {
            var auths = new Auths();
            foreach (var item in auths.List)
            {
                var doc = GetSubjectDocByUuid(item[2]);

                //doc.externalAuthorityUri = item[0];
                doc.substrings[0].externalAuthorityUri = item[0];
                doc.substrings[1].externalAuthorityUri = item[1];
                //doc.substrings[2].externalAuthorityUri = item[2];
                //doc.substrings[3].externalAuthorityUri = item[3];
                //doc.substrings[4].externalAuthorityUri = item[4];

                //doc.substrings[2].authoritativeLabel = item[0];
                //doc.authoritativeLabel = item[1];


                var request = new RestRequest($"subject_authority/{doc._id}/", Method.PUT, DataFormat.Json);
                var json = JsonConvert.SerializeObject(doc, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }
        }

        public void ImportCorporate()
        {
            var corp = new Corporate();
            var response = GetUuids(corp.Data2.Count);

            for (var i = 0; i < corp.Data2.Count; i++)
            {
                var request = new RestRequest($"authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                request.AddJsonBody(new { authoritativeLabel = corp.Data2[i][0], archivesspaceUri = corp.Data2[i][1] });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportCorporateRelations()
        {
            var corp = new Corporate();

            for (var i = 0; i < corp.Relations.Count; i++)
            {
                var request = new RestRequest($"authority/_find", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { selector = new { archivesspaceUri = corp.Relations[i][0] } });
                var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);

                request = new RestRequest($"authority/{doc.Docs[0]._id}/", Method.PUT, DataFormat.Json);

                if (doc.Docs[0].corporateNameSubject == null)
                {
                    doc.Docs[0].corporateNameSubject = new List<string>();
                }
                doc.Docs[0].corporateNameSubject.Add(corp.Relations[i][2]);

                var json = JsonConvert.SerializeObject(doc.Docs[0], new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }
        }

        public void ImportNameAuths()
        {
            var nm = new PersonName();
            for (var i = 0; i < nm.Data3.Count; i++)
            {
                var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { selector = new { authoritativeLabel = nm.Data3[i][0] } });
                var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);

                request = new RestRequest($"name_authority/{doc.Docs[0]._id}/", Method.PUT, DataFormat.Json);

                doc.Docs[0].authoritativeLabel = nm.Data3[i][1];
                doc.Docs[0].externalAuthorityUri = nm.Data3[i][2];
                
                var json = JsonConvert.SerializeObject(doc.Docs[0], new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }
        }

        public void ImportGeographic()
        {
            var geo = new Geographic();
            var response = GetUuids(geo.Data.Count);

            for (var i = 0; i < geo.Data.Count; i++)
            {
                var request = new RestRequest($"authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                request.AddJsonBody(new { authoritativeLabel = geo.Data[i][0], archivesspaceUri = geo.Data[i][1] });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportGeographicRelations()
        {
            var meeting = new Meeting();

            for (var i = 0; i < meeting.Relations.Count; i++)
            {
                var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { selector = new { archivesSpaceUri = meeting.Relations[i][0] } });
                var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);

                request = new RestRequest($"subject_authority/{doc.Docs[0]._id}/", Method.PUT, DataFormat.Json);

                if (doc.Docs[0].uniformTitle == null)
                {
                    doc.Docs[0].uniformTitle = new List<string>();
                }
                doc.Docs[0].uniformTitle.Add(meeting.Relations[i][2]);

                var json = JsonConvert.SerializeObject(doc.Docs[0], new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }

        }

        public void ImportTopical()
        {
            var meeting = new Meeting();
            var response = GetUuids(meeting.Data.Count);

            for (var i = 0; i < meeting.Data.Count; i++)
            {
                var request = new RestRequest($"subject_authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                request.AddJsonBody(new { authoritativeLabel = meeting.Data[i][0], archivesSpaceUri = meeting.Data[i][1] });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportS1s()
        {
            var meeting = new Meeting();
            var response = GetUuids(meeting.Data.Count);

            for (var i = 0; i < meeting.Data.Count; i++)
            {
                var request = new RestRequest($"subject_authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                request.AddJsonBody(new { authoritativeLabel = meeting.Data[i][0], archivesSpaceUri = meeting.Data[i][1], externalAuthorityUri = meeting.Data[i][2] == string.Empty ? null : meeting.Data[i][2] });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportS2s()
        {
            var meeting = new Meeting();
            var response = GetUuids(meeting.Data.Count);

            for (var i = 0; i < meeting.Data.Count; i++)
            {
                var request = new RestRequest($"subject_authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                var subs = new List<Substring>();
                subs.Add(new Substring(meeting.Data[i][2], meeting.Data[i][3]));
                subs.Add(new Substring(meeting.Data[i][4], meeting.Data[i][5]));
                request.AddJsonBody(new { authoritativeLabel = meeting.Data[i][0], archivesSpaceUri = meeting.Data[i][1], substrings = subs });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportS3s()
        {
            var meeting = new Meeting();
            var response = GetUuids(meeting.Data.Count);

            for (var i = 0; i < meeting.Data.Count; i++)
            {
                var request = new RestRequest($"subject_authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                var subs = new List<Substring>();
                subs.Add(new Substring(meeting.Data[i][2], meeting.Data[i][3]));
                subs.Add(new Substring(meeting.Data[i][4], meeting.Data[i][5]));
                subs.Add(new Substring(meeting.Data[i][6], meeting.Data[i][7]));
                request.AddJsonBody(new { authoritativeLabel = meeting.Data[i][0], archivesSpaceUri = meeting.Data[i][1], substrings = subs });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportS4s()
        {
            var meeting = new Meeting();
            var response = GetUuids(meeting.Data.Count);

            for (var i = 0; i < meeting.Data.Count; i++)
            {
                var request = new RestRequest($"subject_authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                var subs = new List<Substring>();
                subs.Add(new Substring(meeting.Data[i][2], meeting.Data[i][3]));
                subs.Add(new Substring(meeting.Data[i][4], meeting.Data[i][5]));
                subs.Add(new Substring(meeting.Data[i][6], meeting.Data[i][7]));
                subs.Add(new Substring(meeting.Data[i][8], meeting.Data[i][9]));
                request.AddJsonBody(new { authoritativeLabel = meeting.Data[i][0], archivesSpaceUri = meeting.Data[i][1], substrings = subs });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportEadRelations()
        {
            var topical = new Meeting();

            for (var i = 0; i < topical.Data.Count; i++)
            {
                var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { selector = new { _id = topical.Data[i][0] } });
                var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);

                request = new RestRequest($"subject_authority/{doc.Docs[0]._id}/", Method.PUT, DataFormat.Json);

                if (doc.Docs[0].uniformTitle == null)
                {
                    doc.Docs[0].uniformTitle = new List<string>();
                    doc.Docs[0].uniformTitle.Add(topical.Data[i][1]);
                }
                else if(!doc.Docs[0].uniformTitle.Contains(topical.Data[i][1]))
                {
                    doc.Docs[0].uniformTitle.Add(topical.Data[i][1]);
                }
                
                var json = JsonConvert.SerializeObject(doc.Docs[0], new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }

        }

        public void Clear()
        {
            for (var i = 1; i <= 5928; i++)
            {
                var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { selector = new { archivesSpaceUri = "http://archivesspace.ecu.edu/subjects/" + i } });
                var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);

                request = new RestRequest($"subject_authority/{doc.Docs[0]._id}/", Method.PUT, DataFormat.Json);

                doc.Docs[0].topic = null;
                doc.Docs[0].geographic = null;
                doc.Docs[0].personalNameSubject = null;
                doc.Docs[0].corporateNameSubject = null;

                var json = JsonConvert.SerializeObject(doc.Docs[0], new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }
        }

        public void ImportTopicRelations()
        {
            var topical = new Topical();

            for (var i = 0; i < topical.Relations6.Count; i++)
            {
                var request = new RestRequest($"subject_authority/_find", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { selector = new { archivesSpaceUri = topical.Relations6[i][0] } });
                var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);

                request = new RestRequest($"subject_authority/{doc.Docs[0]._id}/", Method.PUT, DataFormat.Json);

                if (topical.Relations6[i][1] == "1275")
                {
                    if (doc.Docs[0].topic == null)
                    {
                        doc.Docs[0].topic = new List<string>();
                    }
                    doc.Docs[0].topic.Add(topical.Relations6[i][2]);
                }
                else if (topical.Relations6[i][1] == "1269")
                {
                    if (doc.Docs[0].geographic == null)
                    {
                        doc.Docs[0].geographic = new List<string>();
                    }
                    doc.Docs[0].geographic.Add(topical.Relations6[i][2]);
                }
                else if (topical.Relations6[i][1] == "46580")
                {
                    if (doc.Docs[0].personalNameSubject == null)
                    {
                        doc.Docs[0].personalNameSubject = new List<string>();
                    }
                    doc.Docs[0].personalNameSubject.Add(topical.Relations6[i][2]);
                }
                else if (topical.Relations6[i][1] == "46581")
                {
                    if (doc.Docs[0].corporateNameSubject == null)
                    {
                        doc.Docs[0].corporateNameSubject = new List<string>();
                    }
                    doc.Docs[0].corporateNameSubject.Add(topical.Relations6[i][2]);
                }

                var json = JsonConvert.SerializeObject(doc.Docs[0], new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }

        }

        public void ImportFamilies()
        {
            var families = new FamilyName();
            var response = GetUuids(families.Data.Count);

            for (var i = 0; i < families.Data.Count; i++)
            {
                var request = new RestRequest($"name_authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                request.AddJsonBody(new { authoritativeLabel = families.Data[i][0], archivesSpaceUri = families.Data[i][1] });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportFamilyRelations()
        {
            var families = new FamilyName();

            for (var i = 0; i < families.Relations.Count; i++)
            {
                var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { selector = new { archivesSpaceUri = families.Relations[i][0] } });
                var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);

                request = new RestRequest($"name_authority/{doc.Docs[0]._id}/", Method.PUT, DataFormat.Json);

                if (families.Relations[i][1] == "personalNameCreator")
                {
                    if (doc.Docs[0].familyNameCreator == null)
                    {
                        doc.Docs[0].familyNameCreator = new List<string>();
                    }
                    doc.Docs[0].familyNameCreator.Add(families.Relations[i][2]);
                }
                else
                {
                    if (doc.Docs[0].familyNameSource == null)
                    {
                        doc.Docs[0].familyNameSource = new List<string>();
                    }
                    doc.Docs[0].familyNameSource.Add(families.Relations[i][2]);
                }

                var json = JsonConvert.SerializeObject(doc.Docs[0],new JsonSerializerSettings{NullValueHandling = NullValueHandling.Ignore});
                request.AddJsonBody(json);
                _client.Execute(request);
            }

        }

        public void ImportCorps()
        {
            var families = new CorporateName();
            var response = GetUuids(families.Data.Count);

            for (var i = 0; i < families.Data.Count; i++)
            {
                var request = new RestRequest($"name_authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                request.AddJsonBody(new { authoritativeLabel = families.Data[i][0], archivesSpaceUri = families.Data[i][1] });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportCorpRelations()
        {
            var corps = new CorporateName();

            for (var i = 0; i < corps.Relations.Count; i++)
            {
                var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { selector = new { archivesSpaceUri = corps.Relations[i][0] } });
                var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);

                request = new RestRequest($"name_authority/{doc.Docs[0]._id}/", Method.PUT, DataFormat.Json);

                if (corps.Relations[i][1] == "personalNameCreator")
                {
                    if (doc.Docs[0].corporateNameCreator == null)
                    {
                        doc.Docs[0].corporateNameCreator = new List<string>();
                    }
                    doc.Docs[0].corporateNameCreator.Add(corps.Relations[i][2]);
                }
                else
                {
                    if (doc.Docs[0].corporateNameSource == null)
                    {
                        doc.Docs[0].corporateNameSource = new List<string>();
                    }
                    doc.Docs[0].corporateNameSource.Add(corps.Relations[i][2]);
                }

                var json = JsonConvert.SerializeObject(doc.Docs[0], new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }

        }

        public void ImportPerson()
        {
            var person = new PersonName();
            var response = GetUuids(person.Data2.Count);

            for (var i = 0; i < person.Data2.Count; i++)
            {
                var request = new RestRequest($"name_authority/{response.uuids[i]}", Method.PUT, DataFormat.Json);
                request.AddJsonBody(new { authoritativeLabel = person.Data2[i][0], archivesSpaceUri = person.Data2[i][1] });
                var test = _client.Execute(request).Content;
            }
        }

        public void ImportPersonRelations()
        {
            var persName = new PersonName();

            for (var i = 0; i < persName.Relations.Count; i++)
            {
                var request = new RestRequest($"name_authority/_find", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { selector = new { archivesSpaceUri = persName.Relations[i][0] } });
                var doc = JsonConvert.DeserializeObject<CouchDocs>(_client.Execute(request).Content);

                request = new RestRequest($"name_authority/{doc.Docs[0]._id}/", Method.PUT, DataFormat.Json);

                if (persName.Relations[i][1] == "personalNameCreator")
                {
                    if (doc.Docs[0].personalNameCreator == null)
                    {
                        doc.Docs[0].personalNameCreator = new List<string>();
                    }
                    doc.Docs[0].personalNameCreator.Add(persName.Relations[i][2]);
                }
                else
                {
                    if (doc.Docs[0].personalNameSource == null)
                    {
                        doc.Docs[0].personalNameSource = new List<string>();
                    }
                    doc.Docs[0].personalNameSource.Add(persName.Relations[i][2]);
                }

                var json = JsonConvert.SerializeObject(doc.Docs[0], new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddJsonBody(json);
                _client.Execute(request);
            }

        }
    }
}