import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collection;
import java.util.Locale;
import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.SwingConstants;

/**
 * Represents a Graphical Calendar using Java Swing Framework.
 * @author Alessandro Sanino <alessandro.sanino@edu.unito.it> OR <saninoale@gmail.com>
 * @author Riccardo Krauter <323595@edu.unito.it> OR <riccardo.krauter@gmail.com>
 * @version 1.1
 */
public class JCalendar extends JPanel {

    protected JPanel pane;
    protected JPanel headingPanel;
    protected JCalendarCell selected;
    public final Color COLOR_UNSELECTED_CELL;
    public final Color COLOR_SELECTED_CELL;

    /**
     * Represents an empty Cell in the JCalendar.
     */
    protected static class JEmptyCalendarCell extends JLabel {
        /**
         * Creates a new instance of JEmptyCalendarCell.
         */
        public JEmptyCalendarCell() {
            super(" ");
        }
    }

    /**
     * Represents a single cell in the calendar.
     */
    public static class JCalendarCell extends JPanel {

        /**
         * Represents the calendar owning this cell.
         */
        JCalendar owner;

        /**
         * Represents the Date value of the cell.
         */
        Calendar value;

        /**
         * Creates a new currentDay of JCalendarCell with the specified Date and
         * Owner.
         *
         * @param owner The Calendar owning this cell.
         * @param value The date which is represented by this cell
         */
        public JCalendarCell(JCalendar owner, Calendar value) {
            super();
            this.owner = owner;
            this.value = value;
            this.add(new JLabel(Integer.toString(this.value.get(Calendar.DAY_OF_MONTH))));
            //you can add your custom class which uses method select() from JCalendar class.
            this.addMouseListener(new MouseAdapter() {
                @Override
                public void mousePressed(MouseEvent e) {
                    JCalendarCell sender = (JCalendarCell)e.getSource();
                    (sender).owner.select(sender);
                }
            });
            this.setStyle();
        }

        /**
         * Sets the style of the cell.
         */
        protected void setStyle() {
            this.setBackground(Color.WHITE);
            this.setPreferredSize(new Dimension(0, 50));
            this.setBorder(BorderFactory.createLineBorder(Color.blue));
        }

        /**
         * Gets the date represented by this cell.
         *
         * @return The date represented by this cell.
         */
        public Calendar getValue() {
            return this.value;
        }

        /**
         * Gets the Calendar owning this cell.
         *
         * @return The Calendar owning this cell.
         */
        public JCalendar getOwner() {
            return this.owner;
        }

        @Override
        public boolean equals(Object o) {
            return o instanceof JCalendarCell
                    //exactly same owner (not another one exactly equal)
                    && ((JCalendarCell) o).owner == this.owner
                    //same day
                    && ((JCalendarCell) o).value.get(Calendar.DAY_OF_MONTH) == this.value.get(Calendar.DAY_OF_MONTH)
                    && ((JCalendarCell) o).value.get(Calendar.MONTH) == this.value.get(Calendar.MONTH)
                    && ((JCalendarCell) o).value.get(Calendar.YEAR) == this.value.get(Calendar.YEAR);
        }
    }

    /**
     * Represents the week days expressed in Italian Language.
     */
    public static final String[] ITALIAN_WEEK_DAYS;

    static {
        ITALIAN_WEEK_DAYS = new String[]{"Dom", "Lun", "Mar", "Mer", "Gio", "Ven", "Sab"};
    }

    /**
     * Represents the current date selected.
     */
    protected Calendar currentDay;

    /**
     * Represents the label on top of the calendar (month + year)
     */
    protected JLabel dateLabel;

    /**
     * Represents what to do when a cell is selected.
     */
    protected final Collection<ActionListener> cellSelectedListeners;

    /**
     * Creates a new instance of JCalendar.
     * @param selectedCellColor The color of the cell when it is selected.
     * @param unselectedCellColor The color of the cell when it is not selected.
     */
    public JCalendar(Color selectedCellColor, Color unselectedCellColor) {
        super();
        //today is the selected day.
        currentDay = Calendar.getInstance();
        cellSelectedListeners = new ArrayList<>();
        this.COLOR_SELECTED_CELL = selectedCellColor;
        this.COLOR_UNSELECTED_CELL = unselectedCellColor;
        designInterface();
    }
    
    public JCalendar() {
        this(Color.RED, Color.WHITE);
    }

    /**
     * Designs the interface for the calendar.
     */
    protected void designInterface() {
        this.setLayout(new BorderLayout());
        designHeading();
        designDays();
    }

    /**
     * Designs the interface for the headings of the calendar.
     */
    protected void designHeading() {
        headingPanel = new JPanel();
        headingPanel.setLayout(new BorderLayout());
        JButton previous = new JButton("<");
        JButton next = new JButton(">");
        next.setPreferredSize(new Dimension(50, 50));
        previous.setPreferredSize(new Dimension(50, 50));        
        
        //adds listeners to change months
        previous.addActionListener((ActionEvent e) -> {
            this.aggiornaMese(currentDay.get(Calendar.MONTH) - 1);
        });

        next.addActionListener((ActionEvent e) -> {
            this.aggiornaMese(currentDay.get(Calendar.MONTH) + 1);
        });

        //END
        dateLabel = new JLabel(Integer.toString(currentDay.get(Calendar.DAY_OF_MONTH)) + "-" + Integer.toString(currentDay.get(Calendar.MONTH) + 1) + "-" + Integer.toString(currentDay.get(Calendar.YEAR)));
        dateLabel.setHorizontalAlignment(JLabel.CENTER);
        headingPanel.add(previous, BorderLayout.WEST);
        headingPanel.add(dateLabel);
        headingPanel.add(next, BorderLayout.EAST);    
        this.add(headingPanel, BorderLayout.NORTH);
    }

    /**
     * Gets the current Listener for the CellSelected Event.
     *
     * @return The current Listener for the CellSelected Event.
     */
    public ActionListener[] getCellSelectedListener() {
        return (ActionListener[]) cellSelectedListeners.toArray();
    }
    
    /**
     * Adds an Event to the Cell Selected Events list.
     * @param listener Listener to add.
     */
    public void addCellSelectedListener(ActionListener listener) {
        this.cellSelectedListeners.add(listener);
    }

    /**
     * Clears the Event list for the Cell Selected Event.
     */
    public void clearCellSelectedListeners() {
        this.cellSelectedListeners.clear();
    }

    /**
     * Designs the interface for the days of the calendar.
     */
    protected void designDays() {
        pane = new JPanel();        
        this.add(pane, BorderLayout.CENTER);
        aggiornaMese(currentDay.get(Calendar.MONTH), true);
    }

    /**
     * Updates the month and repaints the Calendar
     *
     * @param month Month to get.
     * @param selectToday if true automatically selects the current day, if
     * false selects the first day of the month.
     */
    protected void aggiornaMese(int month, Boolean selectToday) {
        //hides the panel not to show the lag :D :D :D
        pane.setVisible(false);
        
        pane.removeAll();
        currentDay.set(Calendar.DAY_OF_MONTH, 1);
        currentDay.set(Calendar.MONTH, month);
        String monthName = currentDay.getDisplayName(Calendar.MONTH, Calendar.SHORT, Locale.ITALY);

        Integer year = currentDay.get(Calendar.YEAR),
                startDay = currentDay.get(Calendar.DAY_OF_WEEK),
                numDays = currentDay.getActualMaximum(Calendar.DAY_OF_MONTH);
        //shows the text with month and year
        dateLabel.setText(monthName + " - " + year.toString());

        //rows : num weeks + 1 + one of heading.
        pane.setLayout(new GridLayout(7, 7));
        int i = startDay - 1;
        int startDayWeek = (startDay % 7);
        //heading row
        for (String dayOfWeek : JCalendar.ITALIAN_WEEK_DAYS) {
            JLabel temp = new JLabel(dayOfWeek);
            temp.setHorizontalAlignment(SwingConstants.CENTER);
            pane.add(temp);
        }

        //adds initial empty cells.
        for (int span = startDayWeek; span > Calendar.SUNDAY; span--) {
            pane.add(new JEmptyCalendarCell());
        }

        Calendar time = Calendar.getInstance();
        for (int day = 1; day <= numDays; day++) {
            JCalendarCell temp;
            time.set(year, month, day);
            temp = new JCalendarCell(this, (Calendar) time.clone());

            pane.add(temp);
            if (selectToday) {
                Calendar today = Calendar.getInstance();
                Boolean sameDay = time.get(Calendar.DAY_OF_MONTH) == today.get(Calendar.DAY_OF_MONTH)
                        && time.get(Calendar.MONTH) == today.get(Calendar.MONTH)
                        && time.get(Calendar.YEAR) == today.get(Calendar.YEAR);
                if (sameDay) {
                    select(temp);
                    selectToday = false;
                }
            } else if (this.selected != null && this.selected.equals(temp)) {
                select(temp);
            }
            i++;
        }
        
        //adds final empty cells.
        int compCount = pane.getComponentCount();        
        while (compCount++ < 49) {
            pane.add(new JEmptyCalendarCell());
        }
        
        //shows the result.
        pane.setVisible(true);
    }

    /**
     * Updates the month and repaints the Calendar
     *
     * @param month Month to get.
     */
    protected void aggiornaMese(int month) {
        aggiornaMese(month, false);
    }

    /**
     * Selects a cell in the Calendar.
     *
     * @param cell Cell to select.
     */
    public void select(JCalendarCell cell) {
        this.unselect();
        this.selected = cell;
        this.selected.setBackground(this.COLOR_SELECTED_CELL);
        this.cellSelectedListeners.stream().forEach((ActionListener action) -> {
            //or you can use a custom listener (used ActionListener for Lazyness)
            action.actionPerformed(null);
        });
    }

    /**
     * Unselects the selected cell in the Calendar.
     */
    protected void unselect() {
        if (this.selected != null) {
            this.selected.setBackground(this.COLOR_UNSELECTED_CELL);
        }
    }
    
    /**
     * Returns the selected Cell.
     * @return The selected Cell.
     */
    public JCalendarCell getSelected() {
        return selected;
    }
}
