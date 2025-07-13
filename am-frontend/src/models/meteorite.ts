import type Recclass from './recclass';

export interface Meteorite {
	id: number;
	name: string;
	nameType: string;
	recclass: Recclass;
	mass: number;
	fall: string;
	year: number;
	reclat: number;
	reclong: number;
}
