using System.ComponentModel.DataAnnotations.Schema;

[Table("raw_news")] // Явно указываем имя таблицы
public class RawNewsModel
{
    [Column("id")] // Все названия столбцов в lowercase
    public int Id { get; set; }

    [Column("source")]
    public string Source { get; set; } = string.Empty;

    [Column("text", TypeName = "text")] // Важно указать тип 'text' для неограниченного текста
    public string Text { get; set; } = string.Empty;

    [Column("title", TypeName = "text")] // Аналогично для заголовка
    public string Title { get; set; } = string.Empty;

    [Column("link")]
    public string Link { get; set; } = string.Empty;

    [Column("guid")]
    public string Guid { get; set; } = string.Empty;

    [Column("author")] // Теперь в lowercase
    public string Author { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("is_processed")]
    public bool IsProcessed { get; set; } = false;
}