using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace foodadmin
{
	public class TodoItem
	{
		string id;
		string name;
		bool done;
		DateTime over;
		string place;
		DateTime start;
		string textNumber;
		string userName;
		string passWord;

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value;}

		}

        [JsonProperty(PropertyName = "text")]
		public string Name
		{
			get { return name; }
			set { name = value;}
		}

		[JsonProperty(PropertyName = "complete")]
		public bool Done
		{
			get { return done; }
			set { done = value;}
		}

		[JsonProperty(PropertyName = "expired")]
		public DateTime Over
		{
			get { return over; }
			set { over = value; }
		}

		[JsonProperty(PropertyName = "location")]
		public string Place
		{
			get { return place; }
			set { place = value; }
		}

		[JsonProperty(PropertyName = "begins")]
		public DateTime Start
		{
			get { return start; }
			set { start = value; }
		}

		[JsonProperty(PropertyName = "sms")]
		public string TextNumber
		{
			get { return textNumber; }
			set { textNumber = value; }
		}

		[JsonProperty(PropertyName = "username")]
		public string UserName
		{
			get { return userName; }
			set { userName = value; }

		}

		[JsonProperty(PropertyName = "password")]
		public string PassWord
		{
			get { return passWord; }
			set { passWord = value; }

		}

		[Version]
        public string Version { get; set; }
	}
}


