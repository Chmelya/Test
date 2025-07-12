import type { Meteorite } from './meteorite';

export default interface MeteoritesByYear {
	year: number;
	totalMass: number;
	meteoritesCount: number;
	meteorites: Meteorite[];
}
