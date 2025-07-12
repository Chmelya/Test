export interface PaginatedResponse<T> {
	items: T[];
	pageCount: number;
	pageNumber: number;
	totalCount: number;
}
