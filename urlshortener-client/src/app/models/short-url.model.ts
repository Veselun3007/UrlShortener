export interface ShortUrlDto {
  id: string;
  originalUrl: string;
  shortCode: string;
  createdDate: string;
  createdBy: string;
}

export interface CreateShortUrlRequest {
  url: string;
}
