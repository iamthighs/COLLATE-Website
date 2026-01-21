using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COLLATE.Helpers.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;


namespace COLLATE.Helpers.Models
{
    // Like model class
    public class Like
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public GameAndWebDevModel Post { get; set; }
        public string UserId { get; set; }
    }

    // Comment model class
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public GameAndWebDevModel Post { get; set; }
        public string UserId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public string Content { get; set; }
    }
}
