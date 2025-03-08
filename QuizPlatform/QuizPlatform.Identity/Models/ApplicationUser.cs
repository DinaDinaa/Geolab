using Microsoft.AspNetCore.Identity;
using QuizPlatform.Domain.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Identity.Models;

public class ApplicationUser : IdentityUser
{
    [Column(Order = 7)]
    public bool IsActive { get; set; }
    public virtual List<QuizResult>? QuizResults { get; set; }

}