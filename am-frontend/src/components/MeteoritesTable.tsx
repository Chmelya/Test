import {
	Box,
	Collapse,
	Paper,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
	Typography,
} from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import MeteoritesService from '../api/services/meteoritesService';
import React from 'react';
import IconButton from '@mui/material/IconButton';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import type MeteoritesByYear from '../models/meteoritesByYear';

export default function MeteoritesTable() {
	const { data: rowsPerYear } = useQuery({
		queryKey: ['meteorites', 'getMeteoritesPerYear'],
		queryFn: MeteoritesService.getMeteoritesPerYear,
	});

	if (!rowsPerYear) {
		return <div>LOAD</div>;
	}

	return (
		<TableContainer component={Paper}>
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
					{rowsPerYear.items.map((row) => (
						<Row row={row} />
					))}
				</TableBody>
			</Table>
		</TableContainer>
	);
}

function Row({ row }: { row: MeteoritesByYear }) {
	const [open, setOpen] = React.useState(false);

	return (
		<React.Fragment>
			<TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
				<TableCell>
					<IconButton
						aria-label='expand row'
						size='small'
						onClick={() => setOpen(!open)}
					>
						{open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
					</IconButton>
				</TableCell>
				<TableCell />
				<TableCell>{row.year}</TableCell>
				<TableCell align='right'>{row.totalMass}</TableCell>
				<TableCell align='right'>{row.meteoritesCount}</TableCell>
			</TableRow>
			<TableRow>
				<TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
					<Collapse in={open} timeout='auto' unmountOnExit>
						<Box sx={{ margin: 1 }}>
							<Table size='small' aria-label='purchases'>
								<TableHead>
									<TableRow>
										<TableCell>Name</TableCell>
										<TableCell>Recclass</TableCell>
										<TableCell align='right'>Mass</TableCell>
									</TableRow>
								</TableHead>
								<TableBody>
									{row.meteorites.map((meteorite) => (
										<TableRow>
											<TableCell>{meteorite.name}</TableCell>
											<TableCell>{meteorite.recclass}</TableCell>
											<TableCell align='right'>{meteorite.mass}</TableCell>
										</TableRow>
									))}
								</TableBody>
							</Table>
						</Box>
					</Collapse>
				</TableCell>
			</TableRow>
		</React.Fragment>
	);
}
