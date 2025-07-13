import type { PaginatedRequestParams } from './requests';

export interface MeteoriteSearchFilter extends PaginatedRequestParams {
	startYear?: number;
	endYear?: number;
	recclass?: number;
	namePart?: string;
}
