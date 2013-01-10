using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Mobsway.Utilities;
using Mobsway.Web.Validators;

namespace Mobsway.Business.Entity
{

    #region poll
    [MetadataType(typeof(PollMetaData))]
    public class Poll : BaseEntity
    {

        public string CreatedBy
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public string HashTag
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string PollNumber
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public ImageProviderTypes ImageProvider
        {
            get;
            set;
        }

        public IList<PollOption> Options
        {
            get;
            set;
        }

        [JsonIgnore]
        public int TotalVotes
        {
            get;
            set;
        }

    } 
    #endregion

    public class PollMetaData
    {

        [Required(ErrorMessage="please enter a proper twitter hashtag.")]
        [RegularExpression(@"#([A-Za-z0-9_]+)", ErrorMessage = "please enter a valid Twitter #hashTag.")]
        public string HashTag
        {
            get;
            set;
        }

        [Required(ErrorMessage="please specify a start date for your poll.")]        
        public DateTime StartDate
        {
            get;
            set;
        }

        [Required(ErrorMessage = "please specify a end date for your poll.")]
        [IsDateAfterValidation(OtherDate = "StartDate", AllowEqualDate = true, ErrorMessage = "please choose an end date that is before the start date.")]
        public DateTime EndDate
        {
            get;
            set;
        }

        [StringLength(200, ErrorMessage="please keep the description to under {0} characters.")]
        public string Description
        {
            get;
            set;
        }

    }

    #region polloption
    public class PollOption
    {

        public string HashTag
        {
            get;
            set;
        }

        [JsonIgnore]
        public int TotalVotes
        {
            get;
            set;
        }

    } 
    #endregion

}
