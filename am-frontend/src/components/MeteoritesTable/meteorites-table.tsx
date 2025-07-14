import {
	Paper,
	Stack,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TablePagination,
	TableRow,
	TableSortLabel,
	Toolbar,
	Typography,
} from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import MeteoritesService from '../../api/services/meteoritesService';
import SubRow from './meteorites-table-subRow';
import React, { useState } from 'react';
import type { MeteoriteSearchFilter } from '../../models/requests/meteorites-request';
import FilterMenu from './meteorites-table-filter-menu';
import type { SortOrder } from '../../models/requests/requests';

export default function MeteoritesTable() {
	const [rowsPerPage, setRowsPerPage] = useState<number>(10);
	const [filter, setFilter] = useState<MeteoriteSearchFilter>({
		pageNumber: 1,
		pageSize: rowsPerPage,
	});

	const { data: response } = useQuery({
		queryKey: ['meteorites', 'getMeteoritesPerYear', filter],
		queryFn: () => MeteoritesService.getMeteoritesPerYear(filter),
	});

	//const [sortOrder, setOrder] = useState<SortOrder>('ASC');
	const onColumnChange = (columnName: string) => {
		// if (sortOrder === 'ASC') {
		// 	setOrder('DESC');
		// } else {
		// 	setOrder('ASC');
		// }
		//setFilter({ ...filter, sortColumn: columnName, sortOrder });
	};

	//TODO: Loader
	if (!response) {
		return <div>LOAD</div>;
	}

	return (
		<TableContainer component={Paper} elevation={5}>
			<Toolbar>
				<Stack
					direction='row'
					justifyContent='space-between'
					alignItems='center'
					width='100%'
				>
					<Typography className='' variant='h5'>
						Meteorites
					</Typography>
					<FilterMenu filter={filter} setFilter={setFilter} />
				</Stack>
			</Toolbar>
			<Table aria-label='collapsible table' size='small'>
				<TableHead>
					<TableRow>
						<TableCell>Name</TableCell>
						<TableCell>Status</TableCell>
						<TableCell>Recclass</TableCell>
						<TableCell>Mass</TableCell>
						<TableCell>Reclat</TableCell>
						<TableCell>Reclong</TableCell>
					</TableRow>
				</TableHead>
				<TableBody>
					{response.items.map((row) => (
						<React.Fragment>
							<TableRow sx={{ bgcolor: 'gray' }}>
								<TableCell colSpan={2}>Year: {row.year}</TableCell>
								<TableCell></TableCell>
								<TableCell colSpan={2}>Total mass: {row.totalMass}</TableCell>
								<TableCell>Count: {row.meteoritesCount}</TableCell>
							</TableRow>
							{row.meteorites.map((meteorite) => (
								<TableRow>
									<TableCell>{meteorite.name}</TableCell>
									<TableCell>{meteorite.fall}</TableCell>
									<TableCell>{meteorite.recclass.name}</TableCell>
									<TableCell>{meteorite.mass}</TableCell>
									<TableCell>{meteorite.reclat}</TableCell>
									<TableCell>{meteorite.reclong}</TableCell>
								</TableRow>
							))}
						</React.Fragment>
					))}
				</TableBody>
			</Table>
			<TablePagination
				rowsPerPageOptions={[5, 10, 25]}
				page={response.pageNumber - 1}
				count={response.totalCount}
				rowsPerPage={rowsPerPage}
				onPageChange={(_event, value) => {
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
