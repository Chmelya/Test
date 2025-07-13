export type SortOrder = 'ASC' | 'DESC';

export interface SortRequestParams {
	sortColumn?: string;
	sortOrder?: SortOrder;
}

export interface PaginatedRequestParams extends SortRequestParams {
	pageNumber: number;
	pageSize: number;
}
