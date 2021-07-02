import React, { useState, useEffect, forwardRef } from 'react';
import { useSnackbar } from 'notistack';

// Service
import { searchOrders, downloadOrders, getLabels } from '../services/orderService';

// Components
import Grid from '@material-ui/core/Grid';
import SpeedDial from '@material-ui/lab/SpeedDial';
import SpeedDialIcon from '@material-ui/lab/SpeedDialIcon';
import SpeedDialAction from '@material-ui/lab/SpeedDialAction';
import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import InputLabel from '@material-ui/core/InputLabel';
import Select from '@material-ui/core/Select';
import MenuItem from '@material-ui/core/MenuItem';
import Paper from '@material-ui/core/Paper';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TablePagination from '@material-ui/core/TablePagination';
import TableRow from '@material-ui/core/TableRow';
import Checkbox from '@material-ui/core/Checkbox';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';

import SearchIcon from '@material-ui/icons/Search';
import AppsIcon from '@material-ui/icons/Apps';
import PictureAsPdfIcon from '@material-ui/icons/PictureAsPdf';

import RobotFailSearch from '../components/animations/RobotFailSearch';
import CircularLoadingBackdrop from '../components/animations/loading/CircularLoadingBackdrop';
import Layout from '../components/Layout';
import TableRowOrder from '../components/TableRowOrder';

// Styles
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    boxRobot: {
        height: 400
    },
    speedDial: {
        position: 'fixed',
        bottom: theme.spacing(2),
        right: theme.spacing(2),
        zIndex: 1030,
    },
    paginate: {
        textAlign: 'center'
    },
    formControl: {
        margin: theme.spacing(1)
    },
    select: {
        margin: theme.spacing(1),
        minWidth: 150,
    },
    container: {
        maxHeight: '70vh',
    },
}));

const DEFAULT_BACKDROP = {
    downloading: false,
    label: '',
};

const LIMIT = 50;
const FULFILLMENT = 'fulfillment';
const CROSS_DOCKING = 'cross_docking';
const SELF_SERVICE = 'self_service';
const DEFAULT_FILTERS = { shipping_type: CROSS_DOCKING };
const SHIPPING_TYPES = [
    { name: 'Colecta', value: CROSS_DOCKING },
    { name: 'Full', value: FULFILLMENT },
    { name: 'Flex', value: SELF_SERVICE },
];

const SHIPPING_STATUS = [
    { name: 'Pendiente', value: 'pending' },
    { name: 'En manejo', value: 'handling' },
    { name: 'Listo para envío', value: 'ready_to_ship' },
    { name: 'Enviado', value: 'shipped' },
    { name: 'Entregado', value: 'delivered' },
    { name: 'No entregado', value: 'not_delivered' },
    { name: 'Cancelado', value: 'cancelled' }
];

const OrdersPage = props => {

    const [orders, setOrders] = useState(null);
    const [shippingType, setShippingType] = useState('');
    const [open, setOpen] = useState(false);
    const [page, setPage] = useState(0);
    const [checked, setChecked] = useState([]);
    const [backdrop, setBackdrop] = useState(DEFAULT_BACKDROP);
    const [filters, setFilters] = useState(DEFAULT_FILTERS);

    const classes = useStyles();
    const { enqueueSnackbar } = useSnackbar();

    const sendNotification = (variant, message) => {
        enqueueSnackbar(message, { variant });
    };

    useEffect(() => {
        populateOrders(page);
    }, []);

    const downloadExcel = async () => {
        setOpen(false);
        setBackdrop({ downloading: true, label: 'Descargando Excel, esta acción puede demorar unos minutos...' });
        try {
            const result = await downloadOrders({ ...filters });
            if (result == null) {
                sendNotification('info', 'Sin resultados para descargar el Excel')
            }

            setBackdrop(DEFAULT_BACKDROP);
        } catch (e) {
            setBackdrop(DEFAULT_BACKDROP);
            sendNotification('error', e.message);
        }
    };

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
    };

    const handleCheck = id => {
        const index = checked.findIndex(orderId => orderId === id);
        const newChecked = [ ...checked ];
        if (index === -1 && checked.length < LIMIT) {
            newChecked.push(id);
        } else {
            newChecked.splice(index, 1);
        }

        setChecked(newChecked);
    };

    const onSelectAllClick = evt => {
        if (evt.target.checked) {
            const newChecked = orders.slice(page * 50, page * 50 + 50).map(order => order.orderIDML);
            setChecked(newChecked);
        } else {
            setChecked([]);
        }
    };

    const handleClose = () => setOpen(false);
    const handleOpen = () => setOpen(true);
    const handleChangeStatus = evt => setFilters({ ...filters, shipping_type: evt.target.value });
    const handleDownloadExcel = () => downloadExcel();

    const handleDownloadLabels = async () => {
        if (checked.length === 0) {
            sendNotification('info', 'Debe de seleccionar pedidos para imprimir');
            return;
        }

        const shipments = await checked.map(orderId => {
            const index = orders.findIndex(obj => obj.orderIDML === orderId);
            return orders[index].shippingId;
        });

        setOpen(false);
        setBackdrop({ downloading: true, label: 'Descargando etiquetas, esta acción puede demorar unos minutos...' });
        try {
            await getLabels(shipments);
            setBackdrop(DEFAULT_BACKDROP);
            setChecked([]);
        } catch (e) {
            setBackdrop(DEFAULT_BACKDROP);
            sendNotification('error', e.message);
        }
    };

    const populateOrders = async () => {
        setBackdrop({ downloading: true, label: 'Buscando pedidos' });
        try {
            const data = await searchOrders({ ...filters });
            if (data !== null) {
                setOrders(data.results);
                setShippingType(data.shippingType);
            }
            else {
                sendNotification('info', 'No se pudieron cargar las ordenes');
            }
        } catch (e) {
            sendNotification('error', e.message);
        }

        setBackdrop(DEFAULT_BACKDROP);
    };

    return (
        <Layout {...props}>
            <CircularLoadingBackdrop show={backdrop.downloading} label={backdrop.label} />
            <Grid container justify="center" spacing={1}>
                <Grid item xs={10}>
                    <FormControl color="secondary" className={classes.select}>
                        <InputLabel id="shipping-type-label">
                            Tipo de envío
                        </InputLabel>
                        <Select
                            labelId="shipping-type-label"
                            value={filters.shipping_type}
                            onChange={handleChangeStatus}>
                            {SHIPPING_TYPES.map((type, index) => {
                                return (
                                    <MenuItem key={index} value={type.value}>
                                        {type.name}
                                    </MenuItem>
                                );
                            })}
                        </Select>
                    </FormControl>
                    <Button
                        color="secondary"
                        variant="outlined"
                        className={classes.formControl}
                        onClick={() => populateOrders(1)}
                        startIcon={<SearchIcon />}>
                        Refrescar
                    </Button>
                    {(orders && orders.length > 0) &&
                        <Paper className={classes.root}>
                            <TableContainer className={classes.container}>
                                <Table size="small" stickyHeader aria-label="sticky table">
                                    <TableHead>
                                        <TableRow>
                                            {shippingType !== FULFILLMENT && (
                                                <TableCell padding="checkbox">
                                                    <Checkbox
                                                        indeterminate={checked.length > 0 && checked.length < LIMIT}
                                                        checked={checked.length === LIMIT}
                                                        onChange={onSelectAllClick} />
                                                </TableCell>
                                            )}
                                            <TableCell align="left">Fecha</TableCell>
                                            <TableCell align="left">Pedido</TableCell>
                                            <TableCell align="left">Envío</TableCell>
                                            <TableCell align="left">Nr. Pack</TableCell>
                                            <TableCell align="left">Nombre completo</TableCell>
                                            <TableCell align="left">Productos</TableCell>
                                            <TableCell align="center">Acciones</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {orders.slice(page * 50, page * 50 + 50).map((order, index) => {
                                            const isSelected = checked.findIndex(shipment => shipment === order.orderIDML) !== -1;
                                            const isDisabled = checked.findIndex(shipment => shipment === order.orderIDML) === -1 && checked.length === LIMIT;
                                            const date = format(new Date(order.dateCreated), 'dd/MM/yyyy HH:mm', { locale: es });

                                            return (
                                                <TableRowOrder
                                                    key={order.id}
                                                    order={order}
                                                    date={date}
                                                    selected={isSelected}
                                                    disabled={isDisabled}
                                                    destiny={shippingType === SELF_SERVICE}
                                                    onCheckHandler={handleCheck} />
                                            );
                                        })}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                            <TablePagination
                                rowsPerPageOptions={[50]}
                                component="div"
                                count={orders.length}
                                rowsPerPage={50}
                                page={page}
                                onChangePage={handleChangePage} />
                        </Paper>
                    }
                    {(orders && orders.length === 0) &&
                        <Grid container justify="center" alignItems="center" className={classes.boxRobot}>
                            <RobotFailSearch />
                        </Grid>
                    }
                </Grid>
            </Grid>
            {shippingType !== FULFILLMENT && (
                <SpeedDial
                    ariaLabel="SpeedDial Orders"
                    className={classes.speedDial}
                    hidden={false}
                    icon={<SpeedDialIcon />}
                    onClose={handleClose}
                    onOpen={handleOpen}
                    open={open}
                    direction="up">
                    <SpeedDialAction
                        icon={<AppsIcon />}
                        tooltipTitle="Exportar a Excel"
                        onClick={handleDownloadExcel} />
                    <SpeedDialAction
                        icon={<PictureAsPdfIcon />}
                        tooltipTitle="Descargar Etiquetas"
                        onClick={handleDownloadLabels} />
                </SpeedDial>
            )}
        </Layout>
    );
};

export default OrdersPage;