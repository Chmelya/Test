import {
	Box,
	Button,
	FormControl,
	IconButton,
	InputLabel,
	Menu,
	MenuItem,
	Select,
	Stack,
	TextField,
	Tooltip,
} from '@mui/material';
import { useState } from 'react';
import FilterListIcon from '@mui/icons-material/FilterList';
import type { MeteoriteSearchFilter } from '../../models/requests/meteorites-request';
import MeteoritesService from '../../api/services/meteoritesService';
import { useQuery } from '@tanstack/react-query';
import type { SortOrder } from '../../models/requests/requests';

const FilterMenu = ({
	filter,
	setFilter,
	setOrderBy,
	setSortOrder,
}: {
	filter: MeteoriteSearchFilter;
	setFilter: React.Dispatch<React.SetStateAction<MeteoriteSearchFilter>>;
	setOrderBy: React.Dispatch<React.SetStateAction<SortOrder | undefined>>;
	setSortOrder: React.Dispatch<React.SetStateAction<string | undefined>>;
}) => {
	const [recclass, setRecclass] = useState<string>(
		filter.recclass?.toString() || '0'
	);
	const [name, setName] = useState<string>(filter.namePart || '');
	const [fromYear, setFromYear] = useState<string>(
		filter.startYear?.toString() || ''
	);
	const [toYear, setToYear] = useState<string>(
		filter.endYear?.toString() || ''
	);

	const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
	const isOpened = Boolean(anchorEl);

	const handleClick = (event: React.MouseEvent<HTMLElement>) => {
		setAnchorEl(event.currentTarget);
	};

	const handleClose = () => {
		setAnchorEl(null);
	};

	const applyHandler = () => {
		setFilter((prev) => ({
			...prev,
			recclass: parseInt(recclass) || undefined,
			namePart: name || undefined,
			startYear: fromYear ? parseInt(fromYear) : undefined,
			endYear: toYear ? parseInt(toYear) : undefined,
			pageNumber: 1,
			sortOrder: undefined,
			sortColumn: undefined,
		}));

		setOrderBy(undefined);
		setSortOrder(undefined);

		handleClose();
	};

	const resetHandler = () => {
		setRecclass('0');
		setName('');
		setFromYear('');
		setToYear('');
		setFilter((prev) => ({
			...prev,
			recclass: undefined,
			namePart: undefined,
			startYear: undefined,
			endYear: undefined,
			sortOrder: undefined,
			sortColumn: undefined,
		}));

		setOrderBy(undefined);
		setSortOrder(undefined);

		handleClose();
	};

	const { data: recclasses } = useQuery({
		queryKey: ['meteorites', 'getRecclassesDropDown'],
		queryFn: MeteoritesService.getRecclassesDropDown,
	});

	//TODO: loader
	if (!recclasses) {
		return <></>;
	}

	return (
		<>
			<Tooltip title={'Filter list'}>
				<IconButton onClick={handleClick}>
					<FilterListIcon />
				</IconButton>
			</Tooltip>
			<Menu open={isOpened} anchorEl={anchorEl} onClose={handleClose}>
				<Box sx={{ padding: 2 }}>
					<FormControl fullWidth>
						<Stack direction='column' gap={2}>
							<InputLabel id='recclass-select-label-id'>Recclass</InputLabel>
							<Select
								labelId='recclass-select-label'
								id='recclass-select-Id'
								value={recclass}
								label='Recclass'
								onChange={(event) => setRecclass(event.target.value.toString())}
							>
								<MenuItem value={'0'}>All</MenuItem>
								{recclasses.map((r) => (
									<MenuItem key={r.id} value={r.id}>
										{r.value}
									</MenuItem>
								))}
							</Select>
							<TextField
								id='field-name-id'
								label='Name'
								variant='outlined'
								value={name}
								onChange={(e) => setName(e.target.value)}
							/>
							<Stack direction={'row'} gap={2}>
								<TextField
									id='field-from-year-id'
									label='From year'
									variant='outlined'
									type='number'
									value={fromYear}
									onChange={(e) => setFromYear(e.target.value)}
								/>
								<TextField
									id='field-to-year-id'
									label='To year'
									variant='outlined'
									type='number'
									value={toYear}
									onChange={(e) => setToYear(e.target.value)}
								/>
							</Stack>
							<Stack className='mt-4' direction='row' gap={2}>
								<Button onClick={resetHandler} fullWidth variant='outlined'>
									Reset
								</Button>
								<Button onClick={applyHandler} fullWidth variant='contained'>
									Apply
								</Button>
							</Stack>
						</Stack>
					</FormControl>
				</Box>
			</Menu>
		</>
	);
};
export default FilterMenu;
