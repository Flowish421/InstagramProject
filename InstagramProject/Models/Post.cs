using System;
using System.Collections.Generic;

namespace InstagramProject.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string? Image { get; set; }

    public string? Caption { get; set; }

    public DateTime? Timestamp { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();

    public virtual User? User { get; set; }
}
