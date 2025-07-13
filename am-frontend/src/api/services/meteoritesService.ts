import type MeteoritesByYear from '../../models/meteoritesByYear';
import type { MeteoriteSearchFilter } from '../../models/requests/meteorites-request';
import type {
	DropDownValue,
	PaginatedResponse,
} from '../../models/responses/responses';
import apiClient from '../apiClient';

export default class MeteoritesService {
	static getMeteoritesPerYear = async (params?: MeteoriteSearchFilter) => {
		const res = await apiClient.get<PaginatedResponse<MeteoritesByYear>>(
			'/meteorites/GroupedByYear',
			{ params }
		);

		return res.data;
	};

	static getRecclassesDropDown = async () => {
		const res = await apiClient.get<DropDownValue[]>(
			'/meteorites/options/recclasses'
		);

		return res.data;
	};
}
