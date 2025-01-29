using System;
using System.Collections.Generic;

namespace InstagramProject.Models;

public partial class User
{
    public int UserID { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();
}
