import type { PaginatedRequestParams } from './requests';

export interface MeteoriteFilter extends PaginatedRequestParams {
	startYear?: number;
	endYear?: number;
	recclass?: string;
	namePart?: string;
}
