using System;
namespace DNIDetector.Models
{
	using Newtonsoft.Json;

	public class VerificationResultModel
	{
		[JsonProperty("isIdentical")]
		public bool IsIdentical { get; set; }

		[JsonProperty("confidence")]
		public double Confidence { get; set; }

		[JsonIgnore]
		public double PercentConfidence
		{
			get
			{
				return Confidence * 100;
			}
		}
	}
}
