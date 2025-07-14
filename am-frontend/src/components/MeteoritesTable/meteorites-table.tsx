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

	const [sortOrder, setSortOrder] = useState<SortOrder | undefined>();
	const [orderBy, setOrderBy] = useState<string | undefined>();

	const onCellClick = (orderByNew: string) => {
		const newSortOrder = orderByNew && sortOrder === 'asc' ? 'desc' : 'asc';

		setSortOrder(newSortOrder);
		setOrderBy(orderByNew);

		setFilter((prev) => ({
			...prev,
			sortOrder: newSortOrder,
			sortColumn: orderByNew,
		}));
	};

	interface HeadCell {
		id: string;
		label: string;
	}

	const headCells: readonly HeadCell[] = [
		{
			label: 'Name',
			id: 'name',
		},
		{
			label: 'Recclass',
			id: 'recclassId',
		},
		{
			label: 'Mass',
			id: 'mass',
		},
		{
			label: 'Reclat',
			id: 'reclat',
		},
		{
			label: 'Reclong',
			id: 'reclong',
		},
	];

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
					<FilterMenu
						filter={filter}
						setFilter={setFilter}
						setSortOrder={setSortOrder}
						setOrderBy={setOrderBy}
					/>
				</Stack>
			</Toolbar>
			<Table aria-label='collapsible table' size='small'>
				<TableHead>
					<TableRow>
						{headCells.map((cell, index) => {
							return (
								<TableCell key={index} width={'20%'}>
									<TableSortLabel
										active={orderBy === cell.id}
										direction={sortOrder}
										onClick={() => onCellClick(cell.id)}
									>
										{cell.label}
									</TableSortLabel>
								</TableCell>
							);
						})}
					</TableRow>
				</TableHead>
				<TableBody>
					{response &&
						response.items.map((row) => (
							<React.Fragment>
								<TableRow sx={{ bgcolor: 'gray' }}>
									<TableCell colSpan={2}>Year: {row.year}</TableCell>
									<TableCell colSpan={2}>Total mass: {row.totalMass}</TableCell>
									<TableCell>Count: {row.meteoritesCount}</TableCell>
								</TableRow>
								{row.meteorites.map((meteorite) => (
									<TableRow>
										<TableCell>{meteorite.name}</TableCell>
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
			{response && (
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
			)}
		</TableContainer>
	);
}
