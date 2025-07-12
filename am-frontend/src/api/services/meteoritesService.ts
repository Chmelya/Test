import type MeteoritesByYear from '../../models/meteoritesByYear';
import type { PaginatedResponse } from '../../models/PaginatedResponse';
import apiClient from '../apiClient';

export default class MeteoritesService {
	static getMeteoritesPerYear = async () => {
		const res = await apiClient.get<PaginatedResponse<MeteoritesByYear>>(
			'/meteorites/GroupedByYear'
		);

		return res.data;
	};
}
