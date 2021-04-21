
namespace FreelancerWeb.DataLayer.Models.Server
{
    /// <summary>
    /// Model with status in string
    /// </summary>
    public class StatusUpdateModel
    {
        /// <summary>
        /// Status
        /// </summary>
        /// <example>"processing", "done" or "close"</example>
        /// <remarks>
        /// "processing" work in progress
        /// "done" freelancer have complete work and wait for accepting by customer
        /// "close" accepted by customer
        /// </remarks>
        public string status { get; set; }
    }
}
