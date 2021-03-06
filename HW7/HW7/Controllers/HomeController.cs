﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Configuration;
using System.Net;
using System.IO;
using HW7.Models;
using Newtonsoft.Json.Linq;

namespace HW7.Controllers
{
    public class HomeController : Controller
    {

        private string SendRequest(string uri, string credentials, string username)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("Authorization", "token " + credentials);
            request.UserAgent = username;       // Required, see: https://developer.github.com/v3/#user-agent-required
            request.Accept = "application/json";

            string jsonString = null;
            // TODO: You should handle exceptions here
            using (WebResponse response = request.GetResponse())
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                jsonString = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            return jsonString;
        }
        public class repoInfo //Class that holds repository information
        {

            public string repoName; //= (string)jsonRepos[0]["name"],
            public string repoOwner; //= (string)jsonRepos[0]["owner"]["login"],
            public string repoTime; //= (string)jsonRepos[0]["updated_at"],
            public string repoProfile; //= (string)jsonRepos[0]["owner"]["avatar_url"],
            public string repoUrl; //=(string)jsonRepos[i]["owner"]["html_url"];
        }
        public JsonResult Repos()
        {
            string json = SendRequest("https://api.github.com/user/repos", ConfigurationManager.AppSettings.Get("Token"), "penny993131");

            JArray jsonRepos = JArray.Parse(json);

            //This is a List that holds the type/class repoInfo and the list name is intList
            // new List<repoInfo>(); just creates the list(constructor)
            List<repoInfo> intList = new List<repoInfo>();

            for (var i = 0; i < jsonRepos.Count; i++)
            {
                repoInfo repo = new repoInfo(); //Making a new class to hold current repo information

                //This says "I am getting the [name] from index [i] repository in the array called
                //  jsonRepos and turn it into a string. With this, I am going to assign it to the attribute
                //  repoName inside my repoInfo Object called repo"
                repo.repoName = (string)jsonRepos[i]["name"];
                repo.repoOwner = (string)jsonRepos[i]["owner"]["login"];
                repo.repoTime = (string)jsonRepos[i]["updated_at"];
                repo.repoProfile = (string)jsonRepos[i]["owner"]["avatar_url"];
                repo.repoUrl = (string)jsonRepos[i]["owner"]["html_url"];

                //List.add(new repoInfo)
                intList.Add(repo);

            }
            return Json(intList, JsonRequestBehavior.AllowGet);
        }


        public class commitInfo //Class that holds commit information
        {

            public string commitSha; //= (string)jsonCommit["sha"],
            public string commitTimestamp; //= (string)jsonCommit["commit"]["date"],
            public string commitCommitter; //= (string)jsonCommit["committer"]["name"],
            public string commitCommitmessage; //= (string)jsonCommit["message"],
            public string commitHtmlurl;

        }


        public JsonResult Commits(string owner, string repo)
        {
            string json = SendRequest("https://api.github.com/repos/" + owner + "/" + repo + "/commits", ConfigurationManager.AppSettings.Get("Token"), "penny993131");
            JArray jsonCommit = JArray.Parse(json);
            List<commitInfo> intList = new List<commitInfo>();

            for (var i = 0; i < jsonCommit.Count; i++)
            {
                commitInfo commit = new commitInfo(); //Making a new class to hold current commit information


                commit.commitSha = (string)jsonCommit[i]["sha"];
                commit.commitTimestamp = (string)jsonCommit[i]["commit"]["committer"]["date"];
                commit.commitCommitter = (string)jsonCommit[i]["commit"]["committer"]["name"];
                commit.commitCommitmessage = (string)jsonCommit[i]["commit"]["message"];
                commit.commitHtmlurl = (string)jsonCommit[i]["html_url"];

                //List.add(new commitInfo)
                intList.Add(commit);

            }
            return Json(intList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            //Debug.WriteLine($"My key:{ConfigurationManager.AppSettings.Get("Token")}");
            string json = SendRequest("https://api.github.com/users/penny993131", ConfigurationManager.AppSettings.Get("Token"), "penny993131");
            return View(new userprofile(json));
        }



    }
}