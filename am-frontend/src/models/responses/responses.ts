export interface PaginatedResponse<T> {
	items: T[];
	pageCount: number;
	pageNumber: number;
	totalCount: number;
}

export interface DropDownValue {
	id: number;
	value: string;
}
