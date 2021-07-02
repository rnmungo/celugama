import React, { Fragment } from 'react';

// Components
import { makeStyles } from '@material-ui/core/styles';
import Fab from '@material-ui/core/Fab';
import ChevronLeft from '@material-ui/icons/ChevronLeft';
import ChevronRight from '@material-ui/icons/ChevronRight';

const useStyles = makeStyles(theme => ({
    margin: {
        margin: theme.spacing(1),
    }
}));

const Paginate = ({ disablePrev, disableNext, currentPage, onChangePage }) => {

    const classes = useStyles();

    const nextPage = () => {
        if (!disableNext && onChangePage) {
            onChangePage(currentPage + 1);
        }
    }

    const prevPage = () => {
        if (!disablePrev && onChangePage) {
            onChangePage(currentPage - 1);
        }
    }

    return (
        <Fragment>
            <Fab
                color="primary"
                size="small"
                aria-label="prev"
                className={classes.margin}
                disabled={disablePrev}
                onClick={prevPage}
            >
                <ChevronLeft />
            </Fab>
            <Fab
                color="primary"
                size="small"
                aria-label="next"
                className={classes.margin}
                disabled={disableNext}
                onClick={nextPage}
            >
                <ChevronRight />
            </Fab>
        </Fragment>
    );
}

export default Paginate;