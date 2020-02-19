import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {PieChart, Pie, Cell,Legend} from 'recharts';

export class RedVsBluePieChart extends Component {
    static displayName = RedVsBluePieChart.name;
    constructor(props) {
        super(props);
        this.state = {
            winPerSide: [{ name: "Blue Side", value: props.blueWins }, { name: "Red Side", value: props.redWins }],
        };
    }

    renderCustomizedLabel = ({ cx, cy, midAngle, innerRadius, outerRadius, percent, index }) => {
        const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
        const x = cx + radius * Math.cos(-midAngle * (Math.PI / 180));
        const y = cy + radius * Math.sin(-midAngle * (Math.PI / 180));

        return (
            <text x={x} y={y} fill="white" textAnchor={x > cx ? 'start' : 'end'} dominantBaseline="central">
                {`${(percent * 100).toFixed(2)}%`}
            </text>
        );
    };

    render() {
        const colors = ['#000FFF', '#FF0000'];

        return <PieChart width={800} height={400} >
            <Pie
                data={this.state.winPerSide}
                outerRadius={150}
                fill="#8884d8"
                label={this.renderCustomizedLabel}
                labelLine={false}>
                {this.state.winPerSide.map((entry, index) => <Cell key={index} fill={colors[index % colors.length]} />)}
            </Pie>
            <Legend layout='vertical' align="left" verticalAlign='middle' height={36}/>
        </PieChart>
    }
}

RedVsBluePieChart.propTypes = {
    redWins: PropTypes.number.isRequired,
    blueWins: PropTypes.number.isRequired
};
