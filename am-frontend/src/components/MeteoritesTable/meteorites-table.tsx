import {
	Paper,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TablePagination,
	TableRow,
} from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import MeteoritesService from '../../api/services/meteoritesService';
import SubRow from './subRow';
import { useState } from 'react';
import type { MeteoriteSearchFilter } from '../../models/requests/meteorites-request';

export default function MeteoritesTable() {
	const [filter, setFilter] = useState<MeteoriteSearchFilter>();
	const [rowsPerPage, setRowsPerPage] = useState<number>(10);

	const { data: response } = useQuery({
		queryKey: ['meteorites', 'getMeteoritesPerYear', filter],
		queryFn: () => MeteoritesService.getMeteoritesPerYear(filter),
	});

	if (!response) {
		return <div>LOAD</div>;
	}

	return (
		<TableContainer component={Paper} elevation={5}>
			<Table aria-label='collapsible table'>
				<TableHead>
					<TableRow>
						<TableCell />
						<TableCell>Year</TableCell>
						<TableCell align='right'>Total mass</TableCell>
						<TableCell align='right'>Count</TableCell>
					</TableRow>
				</TableHead>
				<TableBody>
					{response.items.map((row) => (
						<SubRow row={row} />
					))}
				</TableBody>
			</Table>
			<TablePagination
				rowsPerPageOptions={[5, 10, 25]}
				page={response.pageNumber - 1}
				count={response.totalCount}
				rowsPerPage={rowsPerPage}
				onPageChange={(event, value) => {
					setFilter({
						...filter,
						pageNumber: value + 1,
					});
				}}
				onRowsPerPageChange={(event) => {
					setFilter({
						...filter,
						pageNumber: 1,
						pageSize: Number(event.target.value),
					});
					setRowsPerPage(Number(event.target.value));
				}}
			/>
		</TableContainer>
	);
}
