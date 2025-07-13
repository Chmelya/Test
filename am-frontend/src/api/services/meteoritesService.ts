import type MeteoritesByYear from '../../models/meteoritesByYear';
import type { PaginatedResponse } from '../../models/requests/requests';
import apiClient from '../apiClient';

export default class MeteoritesService {
	static getMeteoritesPerYear = async (params?: URLSearchParams) => {
		const res = await apiClient.get<PaginatedResponse<MeteoritesByYear>>(
			'/meteorites/GroupedByYear',
			{ params }
		);

		return res.data;
	};
}
