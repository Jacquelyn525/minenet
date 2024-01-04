const GRID = {
  0:
  {
    TITLE: 'On Shift Miner Location',
    FILTER: '',
  },
  1: {
    TITLE: 'On Shift SAF Location',
    FILTER: '(SAF)',
  },
  2: {
    TITLE: 'On Shift EMT Location',
    FILTER: '(EMT)',
  },
  3: {
    TITLE: 'On Shift CNT Location',
    FILTER: '(CNT)',
  },
  4: {
    TITLE: 'On Shift Equipment Location',
    FILTER: 'EQUIPMENT',
  },
  5: {
    TITLE: 'On Shift Supply Car Location',
    FILTER: 'Jeep',
  },
  6: {
    TITLE: 'On Shift Pad Location',
    FILTER: 'PAD',
  },
};


const setGridTitle = (filterId = 0) => $("#card-title").text(GRID[filterId].TITLE);

const filterGridByTagId = (data, filterId = 0) => data.filter(d => d.lastName.toLowerCase().includes(GRID[filterId].FILTER.toLowerCase()));
