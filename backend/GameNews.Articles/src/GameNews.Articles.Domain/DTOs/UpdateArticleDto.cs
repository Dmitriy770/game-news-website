﻿using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Domain.DTOs;

public record UpdateArticleDto(
    Guid Id,
    string? Title,
    string? PreviewText,
    Guid? PreviewMediaId,
    List<Guid>? Tags,
    string? Content
);